import { ConfirmContextProvider } from '@common/context/ConfirmContext';
import mapValuesWithKeys from '@common/lib/utils/mapValuesWithKeys';
import tableBuilderService, {
  FilterOption,
  IndicatorOption,
  PublicationSubject,
  PublicationSubjectMeta,
  TableData,
  ThemeMeta,
} from '@common/services/tableBuilderService';
import TimePeriod, { TimePeriodCode } from '@common/services/types/TimePeriod';
import { Dictionary } from '@common/types/util';
import Link from '@frontend/components/Link';
import Page from '@frontend/components/Page';
import PreviousStepModalConfirm from '@frontend/modules/table-tool/components/PreviousStepModalConfirm';
import mapValues from 'lodash/mapValues';
import { NextContext } from 'next';
import React, { Component } from 'react';
import DownloadCsvButton from './components/DownloadCsvButton';
import FiltersForm, { FilterFormSubmitHandler } from './components/FiltersForm';
import LocationFiltersForm, {
  LocationFiltersFormSubmitHandler,
} from './components/LocationFiltersForm';
import PublicationForm, {
  PublicationFormSubmitHandler,
} from './components/PublicationForm';
import PublicationSubjectForm, {
  PublicationSubjectFormSubmitHandler,
} from './components/PublicationSubjectForm';
import TimePeriodDataTable from './components/TimePeriodDataTable';
import TimePeriodForm, {
  TimePeriodFormSubmitHandler,
} from './components/TimePeriodForm';
import mapOptionValues from './components/utils/mapOptionValues';
import Wizard from './components/Wizard';
import WizardStep from './components/WizardStep';
import WizardStepHeading from './components/WizardStepHeading';

export interface PublicationOptions {
  id: string;
  title: string;
  topics: {
    id: string;
    title: string;
    publications: {
      id: string;
      title: string;
      slug: string;
    }[];
  }[];
}

interface Props {
  themeMeta: ThemeMeta[];
  publicationId: string;
}

interface State {
  locations: Dictionary<FilterOption[]>;
  startTime?: TimePeriod;
  endTime?: TimePeriod;
  filters: Dictionary<FilterOption[]>;
  indicators: IndicatorOption[];
  publication?: PublicationOptions['topics'][0]['publications'][0];
  subjects: PublicationSubject[];
  subjectId: string;
  subjectName: string;
  subjectMeta: PublicationSubjectMeta;
  timePeriodRange: TimePeriod[];
  tableData: TableData['result'];
}

class TableToolPage extends Component<Props, State> {
  public state: State = {
    filters: {},
    locations: {},
    indicators: [],
    subjectName: '',
    subjectId: '',
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
    subjects: [],
    timePeriodRange: [],
    tableData: [],
  };

  public static async getInitialProps({ query }: NextContext) {
    const themeMeta = await tableBuilderService.getThemes();

    const publication = themeMeta
      .flatMap(option => option.topics)
      .flatMap(option => option.publications)
      .find(option => option.slug === query.publicationSlug);

    return {
      themeMeta,
      publicationId: publication ? publication.id : '',
    };
  }

  private handlePublicationFormSubmit: PublicationFormSubmitHandler = async ({
    publicationId,
  }) => {
    const { themeMeta } = this.props;
    const publication = themeMeta
      .flatMap(option => option.topics)
      .flatMap(option => option.publications)
      .find(option => option.id === publicationId);

    if (!publication) {
      return;
    }

    const { subjects } = await tableBuilderService.getPublicationMeta(
      publicationId,
    );

    this.setState({
      publication,
      subjects,
      subjectName: '',
      tableData: [],
    });
  };

  private handlePublicationSubjectFormSubmit: PublicationSubjectFormSubmitHandler = async ({
    subjectId,
    subjectName,
  }) => {
    const subjectMeta = await tableBuilderService.getPublicationSubjectMeta(
      subjectId,
    );

    this.setState({
      subjectMeta,
      subjectName,
      subjectId,
    });
  };

  private handleLocationFiltersFormSubmit: LocationFiltersFormSubmitHandler = async values => {
    const { subjectId } = this.state;

    const subjectMeta = await tableBuilderService.filterPublicationSubjectMeta({
      ...values,
      subjectId,
    });

    this.setState(prevState => ({
      subjectMeta: {
        ...prevState.subjectMeta,
        timePeriod: subjectMeta.timePeriod,
      },
      locations: mapValuesWithKeys(
        values.locations,
        (locationLevel, locations) =>
          locations.map(
            location =>
              subjectMeta.locations[locationLevel].options.find(
                option => option.value === location,
              ) as FilterOption,
          ),
      ),
    }));
  };

  private handleTimePeriodFormSubmit: TimePeriodFormSubmitHandler = async values => {
    const { subjectId, locations } = this.state;

    const start = TimePeriod.fromString(values.start);
    const end = TimePeriod.fromString(values.end);

    const subjectMeta = await tableBuilderService.filterPublicationSubjectMeta({
      ...mapValues(locations, locationLevel =>
        locationLevel.map(location => location.value),
      ),
      subjectId,
      timePeriod: {
        startYear: start.year,
        startCode: start.code,
        endYear: end.year,
        endCode: end.code,
      },
    });

    this.setState(prevState => ({
      startTime: start,
      endTime: end,
      subjectMeta: {
        ...prevState.subjectMeta,
        filters: subjectMeta.filters,
      },
    }));
  };

  private handleFiltersFormSubmit: FilterFormSubmitHandler = async ({
    filters,
    indicators,
  }) => {
    const {
      subjectId,
      startTime,
      endTime,
      locations,
      subjectMeta,
    } = this.state;

    if (!startTime || !endTime) {
      return;
    }

    const { timePeriodRange, result } = await tableBuilderService.getTableData({
      ...mapValues(locations, locationLevel =>
        locationLevel.map(location => location.value),
      ),
      subjectId,
      indicators,
      filters: Object.values(filters).flat(),
      timePeriod: {
        startYear: startTime.year,
        startCode: startTime.code,
        endYear: endTime.year,
        endCode: endTime.code,
      },
    });

    const filtersByValue = mapValues(subjectMeta.filters, value =>
      mapOptionValues(value.options),
    );

    const indicatorsByValue = mapOptionValues<IndicatorOption>(
      subjectMeta.indicators,
    );

    this.setState({
      filters: mapValuesWithKeys(filters, (filterGroup, selectedFilters) =>
        selectedFilters.map(filter => filtersByValue[filterGroup][filter]),
      ),
      indicators: indicators.map(indicator => indicatorsByValue[indicator]),
      timePeriodRange: timePeriodRange.map(
        timePeriod =>
          new TimePeriod(timePeriod.year, timePeriod.code as TimePeriodCode),
      ),
      tableData: result,
    });
  };

  public render() {
    const { themeMeta, publicationId } = this.props;
    const {
      filters,
      indicators,
      locations,
      publication,
      subjectName,
      subjectMeta,
      subjects,
      timePeriodRange,
      tableData,
    } = this.state;

    return (
      <Page title="Create your own tables online" caption="Table Tool" wide>
        <p>
          Choose the data and area of interest you want to explore and then use
          filters to create your table.
        </p>

        <p>
          Once you've created your table, you can download the data it contains
          for your own offline analysis.
        </p>

        <ConfirmContextProvider>
          {({ askConfirm }) => (
            <>
              <Wizard
                id="tableTool-steps"
                onStepChange={async (nextStep, previousStep) => {
                  if (nextStep < previousStep) {
                    const confirmed = await askConfirm();
                    return confirmed ? nextStep : previousStep;
                  }

                  return nextStep;
                }}
              >
                <WizardStep>
                  {stepProps => (
                    <PublicationForm
                      {...stepProps}
                      publicationId={publicationId}
                      options={themeMeta}
                      onSubmit={this.handlePublicationFormSubmit}
                    />
                  )}
                </WizardStep>
                <WizardStep>
                  {stepProps => (
                    <PublicationSubjectForm
                      {...stepProps}
                      options={subjects}
                      onSubmit={this.handlePublicationSubjectFormSubmit}
                    />
                  )}
                </WizardStep>
                <WizardStep>
                  {stepProps => (
                    <LocationFiltersForm
                      {...stepProps}
                      options={subjectMeta.locations}
                      onSubmit={this.handleLocationFiltersFormSubmit}
                    />
                  )}
                </WizardStep>
                <WizardStep>
                  {stepProps => (
                    <TimePeriodForm
                      {...stepProps}
                      options={subjectMeta.timePeriod.options}
                      onSubmit={this.handleTimePeriodFormSubmit}
                    />
                  )}
                </WizardStep>
                <WizardStep>
                  {stepProps => (
                    <FiltersForm
                      {...stepProps}
                      onSubmit={this.handleFiltersFormSubmit}
                      specification={subjectMeta}
                    />
                  )}
                </WizardStep>
                <WizardStep>
                  {stepProps => (
                    <>
                      <WizardStepHeading {...stepProps}>
                        Explore data
                      </WizardStepHeading>

                      {tableData.length > 0 && (
                        <>
                          <div className="govuk-!-margin-bottom-4">
                            <TimePeriodDataTable
                              filters={filters}
                              indicators={indicators}
                              publicationName={
                                publication ? publication.title : ''
                              }
                              subjectName={subjectName}
                              locations={locations}
                              timePeriods={timePeriodRange}
                              results={tableData}
                            />
                          </div>

                          {publication && (
                            <>
                              <h3>Additional options</h3>

                              <ul className="govuk-list">
                                <li>
                                  <Link
                                    as={`/statistics/${publication.slug}`}
                                    to={`/statistics/publication?publication=${publication.slug}`}
                                  >
                                    Go to publication
                                  </Link>
                                </li>
                                <li>
                                  <DownloadCsvButton
                                    publicationSlug={publication.slug}
                                    meta={subjectMeta}
                                    filters={filters}
                                    indicators={indicators}
                                    locations={locations}
                                    timePeriods={timePeriodRange}
                                    results={tableData}
                                  />
                                </li>

                                <li>
                                  <a href="#api">Access developer API</a>
                                </li>
                                <li>
                                  <Link
                                    as={`/methodologies/${publication.slug}`}
                                    to={`/methodologies/methodology?methodology=${publication.slug}`}
                                  >
                                    Go to methodology
                                  </Link>
                                </li>
                                <li>
                                  <a href="#contact">Contact</a>
                                </li>
                              </ul>
                            </>
                          )}
                        </>
                      )}
                    </>
                  )}
                </WizardStep>
              </Wizard>

              <PreviousStepModalConfirm />
            </>
          )}
        </ConfirmContextProvider>
      </Page>
    );
  }
}

export default TableToolPage;
