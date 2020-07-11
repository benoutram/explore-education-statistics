import { ConfirmContextProvider } from '@common/contexts/ConfirmContext';
import useAsyncRetry from '@common/hooks/useAsyncRetry';
import FiltersForm, {
  FilterFormSubmitHandler,
} from '@common/modules/table-tool/components/FiltersForm';
import LocationFiltersForm, {
  LocationFiltersFormSubmitHandler,
} from '@common/modules/table-tool/components/LocationFiltersForm';
import PreviousStepModalConfirm from '@common/modules/table-tool/components/PreviousStepModalConfirm';
import PublicationForm, {
  PublicationFormSubmitHandler,
} from '@common/modules/table-tool/components/PublicationForm';
import PublicationSubjectForm, {
  PublicationSubjectFormSubmitHandler,
} from '@common/modules/table-tool/components/PublicationSubjectForm';
import TimePeriodForm, {
  TimePeriodFormSubmitHandler,
} from '@common/modules/table-tool/components/TimePeriodForm';
import Wizard from '@common/modules/table-tool/components/Wizard';
import WizardStep from '@common/modules/table-tool/components/WizardStep';
import { FullTable } from '@common/modules/table-tool/types/fullTable';
import { TableHeadersConfig } from '@common/modules/table-tool/types/tableHeaders';
import getDefaultTableHeaderConfig from '@common/modules/table-tool/utils/getDefaultTableHeadersConfig';
import mapFullTable from '@common/modules/table-tool/utils/mapFullTable';
import parseYearCodeTuple from '@common/modules/table-tool/utils/parseYearCodeTuple';
import tableBuilderService, {
  PublicationSubject,
  PublicationSubjectMeta,
  ReleaseTableDataQuery,
  ThemeMeta,
} from '@common/services/tableBuilderService';
import React, { ReactElement, useMemo } from 'react';
import { useImmer } from 'use-immer';

interface Publication {
  id: string;
  title: string;
  slug: string;
}

export interface TableToolState {
  initialStep: number;
  subjectMeta: PublicationSubjectMeta;
  query: ReleaseTableDataQuery;
  response?: {
    table: FullTable;
    tableHeaders: TableHeadersConfig;
  };
}

export interface FinalStepRenderProps {
  publication?: Publication;
  query?: ReleaseTableDataQuery;
  response?: {
    table: FullTable;
    tableHeaders: TableHeadersConfig;
  };
}

export interface TableToolWizardProps {
  themeMeta: ThemeMeta[];
  initialState?: Partial<TableToolState>;
  finalStep?: (props: FinalStepRenderProps) => ReactElement;
  scrollOnMount?: boolean;
}

const TableToolWizard = ({
  themeMeta,
  initialState = {},
  scrollOnMount,
  finalStep,
}: TableToolWizardProps) => {
  const [state, updateState] = useImmer<TableToolState>({
    initialStep: 1,
    subjectMeta: {
      timePeriod: {
        hint: '',
        legend: '',
        options: [],
      },
      locations: {},
      indicators: {},
      filters: {},
    },
    query: {
      subjectId: '',
      indicators: [],
      filters: [],
      locations: {},
    },
    ...initialState,
  });

  const publication = useMemo<Publication | undefined>(() => {
    return themeMeta
      .flatMap(option => option.topics)
      .flatMap(option => option.publications)
      .find(option => option.id === state.query.publicationId);
  }, [state.query.publicationId, themeMeta]);

  const { value: subjects = [], setState: setSubjects } = useAsyncRetry<
    PublicationSubject[]
  >(async () => {
    const { releaseId, publicationId } = state.query;

    if (releaseId) {
      const meta = await tableBuilderService.getReleaseMeta(releaseId);
      return meta.subjects;
    }

    if (publicationId) {
      const meta = await tableBuilderService.getPublicationMeta(publicationId);
      return meta.subjects;
    }

    return [];
  }, [state.query]);

  const handlePublicationFormSubmit: PublicationFormSubmitHandler = async ({
    publicationId: selectedPublicationId,
  }) => {
    const publicationMeta = await tableBuilderService.getPublicationMeta(
      selectedPublicationId,
    );

    setSubjects({
      isLoading: false,
      value: publicationMeta.subjects,
    });

    updateState(draft => {
      draft.query.publicationId = selectedPublicationId;
    });
  };

  const handlePublicationSubjectFormSubmit: PublicationSubjectFormSubmitHandler = async ({
    subjectId: selectedSubjectId,
  }) => {
    const nextSubjectMeta = await tableBuilderService.getPublicationSubjectMeta(
      selectedSubjectId,
    );

    updateState(draft => {
      draft.subjectMeta = nextSubjectMeta;

      draft.query.subjectId = selectedSubjectId;
    });
  };

  const handleLocationStepBack = async () => {
    const { subjectId } = state.query;

    const nextSubjectMeta = await tableBuilderService.getPublicationSubjectMeta(
      subjectId,
    );

    updateState(draft => {
      draft.subjectMeta = nextSubjectMeta;
    });
  };

  const handleLocationFiltersFormSubmit: LocationFiltersFormSubmitHandler = async ({
    locations,
  }) => {
    const nextSubjectMeta = await tableBuilderService.filterPublicationSubjectMeta(
      {
        locations,
        subjectId: state.query.subjectId,
      },
    );

    updateState(draft => {
      draft.subjectMeta.timePeriod = nextSubjectMeta.timePeriod;

      draft.query.locations = locations;
    });
  };

  const handleTimePeriodStepBack = async () => {
    const { subjectId, locations } = state.query;

    const nextSubjectMeta = await tableBuilderService.filterPublicationSubjectMeta(
      {
        subjectId,
        locations,
      },
    );

    updateState(draft => {
      draft.subjectMeta.timePeriod = nextSubjectMeta.timePeriod;
    });
  };

  const handleTimePeriodFormSubmit: TimePeriodFormSubmitHandler = async values => {
    const [startYear, startCode] = parseYearCodeTuple(values.start);
    const [endYear, endCode] = parseYearCodeTuple(values.end);

    const nextSubjectMeta = await tableBuilderService.filterPublicationSubjectMeta(
      {
        ...state.query,
        subjectId: state.query.subjectId,
        timePeriod: {
          startYear,
          startCode,
          endYear,
          endCode,
        },
      },
    );

    updateState(draft => {
      draft.subjectMeta.indicators = nextSubjectMeta.indicators;
      draft.subjectMeta.filters = nextSubjectMeta.filters;

      draft.query.timePeriod = {
        startYear,
        startCode,
        endYear,
        endCode,
      };
    });
  };

  const handleFiltersStepBack = async () => {
    const { subjectId, locations, timePeriod } = state.query;

    const nextSubjectMeta = await tableBuilderService.filterPublicationSubjectMeta(
      {
        subjectId,
        locations,
        timePeriod,
      },
    );

    updateState(draft => {
      draft.subjectMeta.indicators = nextSubjectMeta.indicators;
      draft.subjectMeta.filters = nextSubjectMeta.filters;
    });
  };

  const handleFiltersFormSubmit: FilterFormSubmitHandler = async ({
    filters,
    indicators,
  }) => {
    updateState(draft => {
      draft.response = undefined;
    });

    const query: ReleaseTableDataQuery = {
      ...state.query,
      indicators,
      filters: Object.values(filters).flat(),
    };

    const tableData = await tableBuilderService.getTableData(query);

    if (!tableData.results.length || !tableData.subjectMeta) {
      throw new Error(
        'No data available for the options selected. Please try again with different options.',
      );
    }

    const table = mapFullTable(tableData);
    const tableHeaders = getDefaultTableHeaderConfig(table.subjectMeta);

    updateState(draft => {
      draft.query = query;
      draft.response = {
        table,
        tableHeaders,
      };
    });
  };

  return (
    <ConfirmContextProvider>
      {({ askConfirm }) => (
        <>
          <Wizard
            scrollOnMount={scrollOnMount}
            initialStep={state.initialStep}
            id="tableToolWizard"
            onStepChange={async (nextStep, previousStep) => {
              if (nextStep < previousStep) {
                const confirmed = await askConfirm();
                return confirmed ? nextStep : previousStep;
              }

              return nextStep;
            }}
          >
            {!state.query.releaseId && (
              <WizardStep>
                {stepProps => (
                  <PublicationForm
                    {...stepProps}
                    initialValues={{
                      publicationId: state.query.publicationId ?? '',
                    }}
                    options={themeMeta}
                    onSubmit={handlePublicationFormSubmit}
                  />
                )}
              </WizardStep>
            )}
            <WizardStep>
              {stepProps => (
                <PublicationSubjectForm
                  {...stepProps}
                  initialValues={{
                    subjectId: state.query.subjectId,
                  }}
                  options={subjects}
                  onSubmit={handlePublicationSubjectFormSubmit}
                />
              )}
            </WizardStep>
            <WizardStep onBack={handleLocationStepBack}>
              {stepProps => (
                <LocationFiltersForm
                  {...stepProps}
                  initialValues={state.query.locations}
                  options={state.subjectMeta.locations}
                  onSubmit={handleLocationFiltersFormSubmit}
                />
              )}
            </WizardStep>
            <WizardStep onBack={handleTimePeriodStepBack}>
              {stepProps => (
                <TimePeriodForm
                  {...stepProps}
                  initialValues={{
                    timePeriod: state.query.timePeriod,
                  }}
                  options={state.subjectMeta.timePeriod.options}
                  onSubmit={handleTimePeriodFormSubmit}
                />
              )}
            </WizardStep>
            <WizardStep onBack={handleFiltersStepBack}>
              {stepProps => (
                <FiltersForm
                  {...stepProps}
                  initialValues={{
                    indicators: state.query.indicators,
                    filters: state.query.filters,
                  }}
                  subjectMeta={state.subjectMeta}
                  onSubmit={handleFiltersFormSubmit}
                />
              )}
            </WizardStep>
            {finalStep &&
              finalStep({
                query: state.query,
                response: state.response,
                publication,
              })}
          </Wizard>

          <PreviousStepModalConfirm />
        </>
      )}
    </ConfirmContextProvider>
  );
};

export default TableToolWizard;
