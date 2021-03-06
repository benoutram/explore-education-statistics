import PreReleaseTableToolPage from '@admin/pages/release/pre-release/PreReleaseTableToolPage';
import {
  preReleaseTableToolRoute,
  PreReleaseTableToolRouteParams,
} from '@admin/routes/preReleaseRoutes';
import _dataBlockService, {
  ReleaseDataBlock,
} from '@admin/services/dataBlockService';
import _publicationService, {
  BasicPublicationDetails,
} from '@admin/services/publicationService';
import _tableBuilderService, {
  Release,
  SubjectMeta,
  TableDataResponse,
} from '@common/services/tableBuilderService';
import { render, screen, waitFor, within } from '@testing-library/react';
import React from 'react';
import { MemoryRouter, Route } from 'react-router';
import { generatePath } from 'react-router-dom';

jest.mock('@admin/services/dataBlockService');
jest.mock('@admin/services/publicationService');
jest.mock('@common/services/tableBuilderService');

const dataBlockService = _dataBlockService as jest.Mocked<
  typeof _dataBlockService
>;
const publicationService = _publicationService as jest.Mocked<
  typeof _publicationService
>;
const tableBuilderService = _tableBuilderService as jest.Mocked<
  typeof _tableBuilderService
>;

describe('PreReleaseTableToolPage', () => {
  const testSubjectMeta: SubjectMeta = {
    filters: {
      SchoolType: {
        totalValue: '',
        hint: 'Filter by school type',
        legend: 'School type',
        name: 'school_type',
        options: {
          Default: {
            label: 'Default',
            options: [
              {
                label: 'State-funded primary',
                value: 'state-funded-primary',
              },
              {
                label: 'State-funded secondary',
                value: 'state-funded-secondary',
              },
            ],
          },
        },
      },
    },
    indicators: {
      AbsenceFields: {
        label: 'Absence fields',
        options: [
          {
            value: 'authorised-absence-sessions',
            label: 'Number of authorised absence sessions',
            unit: '',
            name: 'sess_authorised',
            decimalPlaces: 2,
          },
        ],
      },
    },
    locations: {
      localAuthority: {
        legend: 'Local authority',
        options: [
          { value: 'barnet', label: 'Barnet' },
          { value: 'barnsley', label: 'Barnsley' },
        ],
      },
    },
    timePeriod: {
      legend: 'Time period',
      options: [{ label: '2014/15', code: 'AY', year: 2014 }],
    },
  };

  const testTableData: TableDataResponse = {
    subjectMeta: {
      filters: {
        SchoolType: {
          totalValue: '',
          hint: 'Filter by school type',
          legend: 'School type',
          options: {
            Default: {
              label: 'Default',
              options: [
                {
                  label: 'State-funded primary',
                  value: 'state-funded-primary',
                },
                {
                  label: 'State-funded secondary',
                  value: 'state-funded-secondary',
                },
              ],
            },
          },
          name: 'school_type',
        },
      },
      footnotes: [],
      indicators: [
        {
          label: 'Number of authorised absence sessions',
          unit: '',
          value: 'authorised-absence-sessions',
          name: 'sess_authorised',
        },
      ],
      locations: [
        { level: 'localAuthority', label: 'Barnet', value: 'barnet' },
        { level: 'localAuthority', label: 'Barnsley', value: 'barnsley' },
      ],
      boundaryLevels: [],
      publicationName: 'Pupil absence',
      subjectName: 'Absence by characteristic',
      timePeriodRange: [{ code: 'AY', label: '2014/15', year: 2014 }],
      geoJsonAvailable: false,
    },
    results: [
      {
        filters: ['state-funded-primary'],
        geographicLevel: 'localAuthority',
        location: {
          localAuthority: { code: 'barnet', name: 'Barnet' },
        },
        measures: {
          'authorised-absence-sessions': '2613',
        },
        timePeriod: '2014_AY',
      },
      {
        filters: ['state-funded-secondary'],
        geographicLevel: 'localAuthority',
        location: {
          localAuthority: { code: 'barnsley', name: 'Barnsley' },
        },
        measures: {
          'authorised-absence-sessions': 'x',
        },
        timePeriod: '2014_AY',
      },
      {
        filters: ['state-funded-secondary'],
        geographicLevel: 'localAuthority',
        location: {
          localAuthority: { code: 'barnet', name: 'Barnet' },
        },
        measures: {
          'authorised-absence-sessions': '1939',
        },
        timePeriod: '2014_AY',
      },
      {
        filters: ['state-funded-primary'],
        geographicLevel: 'localAuthority',
        location: {
          localAuthority: { code: 'barnsley', name: 'Barnsley' },
        },
        measures: {
          'authorised-absence-sessions': '39',
        },
        timePeriod: '2014_AY',
      },
    ],
  };

  const testPublication: BasicPublicationDetails = {
    id: 'publication-1',
    title: 'Pupil absence',
    slug: 'pupil-absence',
    themeId: 'theme-1',
    topicId: 'topic-1',
    legacyReleases: [],
  };

  const testRelease: Release = {
    id: 'release-1',
    highlights: [
      {
        id: 'block-1',
        name: 'Test highlight',
        description: 'Test highlight description',
      },
    ],
    subjects: [
      {
        id: 'subject-1',
        name: 'Test subject',
        content: '<p>Test content</p>',
        timePeriods: {
          from: '2018',
          to: '2020',
        },
        geographicLevels: ['National'],
      },
    ],
  };

  const testDataBlock: ReleaseDataBlock = {
    id: 'block-1',
    name: 'Test block',
    highlightName: 'Test highlight name',
    source: '',
    heading: '',
    table: {
      tableHeaders: {
        columnGroups: [
          [
            { value: 'barnet', type: 'Location', level: 'localAuthority' },
            { value: 'barnsley', type: 'Location', level: 'localAuthority' },
          ],
        ],
        rowGroups: [
          [
            { value: 'state-funded-primary', type: 'Filter' },
            { value: 'state-funded-secondary', type: 'Filter' },
          ],
        ],
        columns: [{ value: '2014_AY', type: 'TimePeriod' }],
        rows: [{ value: 'authorised-absence-sessions', type: 'Indicator' }],
      },
      indicators: [],
    },
    charts: [],
    query: {
      subjectId: 'subject-1',
      indicators: ['authorised-absence-sessions'],
      filters: ['state-funded-primary', 'state-funded-secondary'],
      timePeriod: {
        startYear: 2014,
        startCode: 'AY',
        endYear: 2014,
        endCode: 'AY',
      },
      locations: {
        localAuthority: ['barnet', 'barnsley'],
      },
    },
  };

  test('renders correctly on step 1 with subjects and highlights', async () => {
    publicationService.getPublication.mockResolvedValue(testPublication);
    tableBuilderService.getRelease.mockResolvedValue(testRelease);

    renderPage();

    await waitFor(() => {
      expect(screen.getByTestId('wizardStep-1')).toHaveAttribute(
        'aria-current',
        'step',
      );

      const step1 = within(screen.getByTestId('wizardStep-1'));
      const tabs = step1.getAllByRole('tabpanel', { hidden: true });

      expect(tabs).toHaveLength(2);

      expect(
        within(tabs[0]).getByRole('heading', { name: 'Choose a table' }),
      ).toBeInTheDocument();
      expect(
        within(tabs[0]).getByRole('link', { name: 'Test highlight' }),
      ).toHaveAttribute(
        'href',
        '/publication/publication-1/release/release-1/prerelease/table-tool/block-1',
      );

      expect(
        within(tabs[1]).getByLabelText('Test subject'),
      ).toBeInTheDocument();

      expect(screen.queryByTestId('dataTableCaption')).not.toBeInTheDocument();
      expect(screen.queryByRole('table')).not.toBeInTheDocument();
    });
  });

  test('renders correctly on step 1 without highlights', async () => {
    publicationService.getPublication.mockResolvedValue(testPublication);
    tableBuilderService.getRelease.mockResolvedValue({
      ...testRelease,
      highlights: [],
    });

    renderPage();

    await waitFor(() => {
      expect(screen.getByTestId('wizardStep-1')).toHaveAttribute(
        'aria-current',
        'step',
      );

      const step1 = within(screen.getByTestId('wizardStep-1'));

      expect(step1.getByLabelText('Test subject')).toBeInTheDocument();
      expect(
        step1.queryByRole('heading', { name: 'Table highlights' }),
      ).not.toBeInTheDocument();

      expect(screen.queryByTestId('dataTableCaption')).not.toBeInTheDocument();
      expect(screen.queryByRole('table')).not.toBeInTheDocument();
    });
  });

  test('renders correctly on step 5 with `dataBlockId` route param', async () => {
    publicationService.getPublication.mockResolvedValue(testPublication);
    dataBlockService.getDataBlock.mockResolvedValue(testDataBlock);

    tableBuilderService.getRelease.mockResolvedValue(testRelease);
    tableBuilderService.getTableData.mockResolvedValue(testTableData);
    tableBuilderService.getSubjectMeta.mockResolvedValue(testSubjectMeta);

    renderPage([
      generatePath<PreReleaseTableToolRouteParams>(
        preReleaseTableToolRoute.path,
        {
          publicationId: 'publication-1',
          releaseId: 'release-1',
          dataBlockId: 'block-1',
        },
      ),
    ]);

    await waitFor(() => {
      expect(screen.getByTestId('wizardStep-5')).toHaveAttribute(
        'aria-current',
        'step',
      );

      expect(screen.getByTestId('dataTableCaption')).toHaveTextContent(
        /Table showing Number of authorised absence sessions for 'Absence by characteristic'/,
      );

      expect(screen.getByRole('table')).toBeInTheDocument();
      expect(screen.getAllByRole('row')).toHaveLength(3);
      expect(screen.getAllByRole('cell')).toHaveLength(5);

      expect(
        screen.getByRole('button', {
          name: 'Download the underlying data of this table (CSV)',
        }),
      ).toBeInTheDocument();
      expect(
        screen.getByRole('button', {
          name: 'Download table as Excel spreadsheet (XLSX)',
        }),
      ).toBeInTheDocument();
    });
  });

  const renderPage = (
    initialEntries: string[] = [
      generatePath<PreReleaseTableToolRouteParams>(
        preReleaseTableToolRoute.path,
        {
          publicationId: 'publication-1',
          releaseId: 'release-1',
        },
      ),
    ],
  ) => {
    return render(
      <MemoryRouter initialEntries={initialEntries}>
        <Route
          component={PreReleaseTableToolPage}
          path={preReleaseTableToolRoute.path}
        />
      </MemoryRouter>,
    );
  };
});
