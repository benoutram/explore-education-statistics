/* eslint-disable no-shadow */
import { ConfirmContextProvider } from '@common/context/ConfirmContext';
import tableBuilderService, {
  PublicationSubject,
  PublicationSubjectMeta,
  TableDataQuery,
  ThemeMeta,
} from '@common/modules/full-table/services/tableBuilderService';
import { FullTable } from '@common/modules/full-table/types/fullTable';
import { TableHeadersConfig } from '@common/modules/full-table/utils/tableHeaders';
import parseYearCodeTuple from '@common/modules/full-table/utils/TimePeriod';
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
import React, { ReactElement, useEffect, useState } from 'react';
import { useImmer } from 'use-immer';
import {
  executeTableQuery,
  getDefaultSubjectMeta,
  initialiseFromQuery,
} from './utils/tableToolHelpers';

interface Publication {
  id: string;
  title: string;
  slug: string;
}

export interface TableToolState {
  initialStep: number;
  subjectMeta: PublicationSubjectMeta;
  query: TableDataQuery;
  response?: {
    table: FullTable;
    tableHeaders: TableHeadersConfig;
  };
}

export interface FinalStepProps {
  publication?: Publication;
  table?: FullTable;
  query?: TableDataQuery;
  tableHeaders?: TableHeadersConfig;
}

export interface TableToolWizardProps {
  themeMeta: ThemeMeta[];
  publicationId?: string;
  releaseId?: string;
  initialQuery?: TableDataQuery;
  finalStep?: (props: FinalStepProps) => ReactElement;
  onTableCreated?: (response: {
    query: TableDataQuery;
    table: FullTable;
    tableHeaders: TableHeadersConfig;
  }) => void;
  onInitialQueryLoaded?: () => void;
}

const TableToolWizard = ({
  themeMeta,
  publicationId,
  releaseId,
  initialQuery,
  finalStep,
  onTableCreated,
  onInitialQueryLoaded,
}: TableToolWizardProps) => {
  const [publication, setPublication] = useState<Publication>();
  const [subjects, setSubjects] = useState<PublicationSubject[]>([]);
  const [isInitialising, setInitialising] = useState(false);

  const [tableToolState, updateTableToolState] = useImmer<TableToolState>({
    initialStep: 1,
    subjectMeta: getDefaultSubjectMeta(),
    query: {
      subjectId: '',
      indicators: [],
      filters: [],
    },
  });

  useEffect(() => {
    if (releaseId) {
      tableBuilderService
        .getReleaseMeta(releaseId)
        .then(({ subjects: releaseSubjects }) => {
          setSubjects(releaseSubjects);
        });
    }
  }, [releaseId]);

  useEffect(() => {
    if (!isInitialising) {
      return;
    }

    setInitialising(true);

    initialiseFromQuery(initialQuery, releaseId).then(state => {
      setInitialising(false);
      updateTableToolState(() => state);

      if (onInitialQueryLoaded) {
        onInitialQueryLoaded();
      }
    });
  }, [
    initialQuery,
    isInitialising,
    onInitialQueryLoaded,
    releaseId,
    updateTableToolState,
  ]);

  const handlePublicationFormSubmit: PublicationFormSubmitHandler = async ({
    publicationId: selectedPublicationId,
  }) => {
    const selectedPublication = themeMeta
      .flatMap(option => option.topics)
      .flatMap(option => option.publications)
      .find(option => option.id === selectedPublicationId);

    if (!selectedPublication) {
      return;
    }

    const {
      subjects: publicationSubjects,
    } = await tableBuilderService.getPublicationMeta(selectedPublicationId);

    setSubjects(publicationSubjects);
    setPublication(selectedPublication);
  };

  const handlePublicationSubjectFormSubmit: PublicationSubjectFormSubmitHandler = async ({
    subjectId: selectedSubjectId,
  }) => {
    const nextSubjectMeta = await tableBuilderService.getPublicationSubjectMeta(
      selectedSubjectId,
    );

    updateTableToolState(draft => {
      draft.subjectMeta = nextSubjectMeta;

      draft.query.subjectId = selectedSubjectId;
    });
  };

  const handleLocationFiltersFormSubmit: LocationFiltersFormSubmitHandler = async ({
    locations,
  }) => {
    const nextSubjectMeta = await tableBuilderService.filterPublicationSubjectMeta(
      {
        ...locations,
        subjectId: tableToolState.query.subjectId,
      },
    );

    updateTableToolState(draft => {
      draft.subjectMeta.timePeriod = nextSubjectMeta.timePeriod;

      draft.query = {
        ...locations,
        ...draft.query,
      };
    });
  };

  const handleTimePeriodFormSubmit: TimePeriodFormSubmitHandler = async values => {
    const [startYear, startCode] = parseYearCodeTuple(values.start);
    const [endYear, endCode] = parseYearCodeTuple(values.end);

    const nextSubjectMeta = await tableBuilderService.filterPublicationSubjectMeta(
      {
        ...tableToolState.query,
        subjectId: tableToolState.query.subjectId,
        timePeriod: {
          startYear,
          startCode,
          endYear,
          endCode,
        },
      },
    );

    updateTableToolState(draft => {
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

  const handleFiltersFormSubmit: FilterFormSubmitHandler = async ({
    filters,
    indicators,
  }) => {
    const query: TableDataQuery = {
      ...tableToolState.query,
      indicators,
      filters: Object.values(filters).flat(),
    };

    const response = await executeTableQuery(query, releaseId);

    updateTableToolState(draft => {
      draft.query = query;
      draft.response = response;
    });

    if (onTableCreated) {
      onTableCreated({
        ...response,
        query,
      });
    }
  };

  return (
    <ConfirmContextProvider>
      {({ askConfirm }) => (
        <>
          <Wizard
            initialStep={tableToolState.initialStep}
            id="tableTool-steps"
            onStepChange={async (nextStep, previousStep) => {
              if (nextStep < previousStep) {
                const confirmed = await askConfirm();
                return confirmed ? nextStep : previousStep;
              }

              return nextStep;
            }}
          >
            {releaseId === undefined && (
              <WizardStep>
                {stepProps => (
                  <PublicationForm
                    {...stepProps}
                    publicationId={publicationId}
                    publicationTitle={publication ? publication.title : ''}
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
                  options={subjects}
                  initialValues={tableToolState.query}
                  onSubmit={handlePublicationSubjectFormSubmit}
                />
              )}
            </WizardStep>
            <WizardStep>
              {stepProps => (
                <LocationFiltersForm
                  {...stepProps}
                  options={tableToolState.subjectMeta.locations}
                  initialValues={tableToolState.query}
                  onSubmit={handleLocationFiltersFormSubmit}
                />
              )}
            </WizardStep>
            <WizardStep>
              {stepProps => (
                <TimePeriodForm
                  {...stepProps}
                  initialValues={tableToolState.query}
                  options={tableToolState.subjectMeta.timePeriod.options}
                  onSubmit={handleTimePeriodFormSubmit}
                />
              )}
            </WizardStep>
            <WizardStep>
              {stepProps => (
                <FiltersForm
                  {...stepProps}
                  onSubmit={handleFiltersFormSubmit}
                  initialValues={tableToolState.query}
                  subjectMeta={tableToolState.subjectMeta}
                />
              )}
            </WizardStep>
            {finalStep &&
              finalStep({
                ...tableToolState.response,
                query: tableToolState.query,
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
