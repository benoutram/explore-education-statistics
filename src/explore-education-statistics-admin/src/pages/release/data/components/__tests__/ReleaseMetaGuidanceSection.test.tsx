import ReleaseMetaGuidanceSection from '@admin/pages/release/data/components/ReleaseMetaGuidanceSection';
import _releaseMetaGuidanceService, {
  ReleaseMetaGuidance,
} from '@admin/services/releaseMetaGuidanceService';
import { render, screen, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import React from 'react';

jest.mock('@admin/services/releaseMetaGuidanceService');

const releaseMetaGuidanceService = _releaseMetaGuidanceService as jest.Mocked<
  typeof _releaseMetaGuidanceService
>;

describe('ReleaseMetaGuidanceSection', () => {
  const testMetaGuidance: ReleaseMetaGuidance = {
    id: 'release-1',
    content: '<p>Test main content</p>',
    subjects: [
      {
        id: 'subject-1',
        name: 'Subject 1',
        filename: 'subject-1.csv',
        content: '<p>Test subject 1 content</p>',
        geographicLevels: ['Local Authority', 'National'],
        timePeriods: {
          from: '2018',
          to: '2019',
        },
        variables: [
          { value: 'filter_1', label: 'Filter 1' },
          { value: 'indicator_1', label: 'Indicator 1' },
        ],
      },
      {
        id: 'subject-2',
        name: 'Subject 2',
        filename: 'subject-2.csv',
        content: '<p>Test subject 2 content</p>',
        geographicLevels: ['Regional', 'Ward'],
        timePeriods: {
          from: '2020',
          to: '2021',
        },
        variables: [
          { value: 'filter_2', label: 'Filter 2' },
          { value: 'indicator_2', label: 'Indicator 2' },
        ],
      },
    ],
  };

  describe('can update release', () => {
    test('renders correct edit buttons', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(
          screen.getByRole('button', { name: 'Save guidance' }),
        ).toBeInTheDocument();
        expect(
          screen.getByRole('button', { name: 'Preview guidance' }),
        ).toBeInTheDocument();
      });
    });

    test('renders empty guidance correctly', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue({
        ...testMetaGuidance,
        content: '',
      });

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(
          screen.getByLabelText('Main guidance content'),
        ).toBeInTheDocument();
      });

      const mainGuidanceContent = screen.getByLabelText(
        'Main guidance content',
      ) as HTMLTextAreaElement;

      expect(mainGuidanceContent).toHaveDisplayValue('<h3>Description</h3>');
      expect(mainGuidanceContent).toHaveDisplayValue('<h3>Coverage</h3>');
      expect(mainGuidanceContent).toHaveDisplayValue(
        '<h3>File formats and conventions</h3>',
      );
    });

    test('renders correct message when there are no subjects', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue({
        ...testMetaGuidance,
        subjects: [],
      });

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(
          screen.getByText(
            'Before you can change the public metadata guidance, you must upload at least one data file.',
          ),
        ).toBeInTheDocument();
      });

      expect(
        screen.queryByLabelText('Main guidance content'),
      ).not.toBeInTheDocument();
      expect(screen.queryByText('Data files')).not.toBeInTheDocument();
      expect(screen.queryAllByRole('accordionSection')).toHaveLength(0);
    });

    test('renders existing guidance with subjects', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(
          screen.getByLabelText('Main guidance content'),
        ).toBeInTheDocument();
      });

      expect(screen.getByLabelText('Main guidance content')).toHaveValue(
        '<p>Test main content</p>',
      );

      const subjects = screen.getAllByTestId('accordionSection');

      // Subject 1

      const subject1 = within(subjects[0]);

      expect(subject1.getByTestId('Filename')).toHaveTextContent(
        'subject-1.csv',
      );
      expect(subject1.getByTestId('Geographic levels')).toHaveTextContent(
        'Local Authority; National',
      );
      expect(subject1.getByTestId('Time period')).toHaveTextContent(
        '2018 to 2019',
      );

      expect(subject1.getByLabelText('File guidance content')).toHaveValue(
        '<p>Test subject 1 content</p>',
      );

      userEvent.click(
        subject1.getByRole('button', {
          name: 'Variable names and descriptions',
        }),
      );

      const section1VariableRows = subject1.getAllByRole('row');

      const section1VariableRow1Cells = within(
        section1VariableRows[1],
      ).getAllByRole('cell');

      expect(section1VariableRow1Cells[0]).toHaveTextContent('filter_1');
      expect(section1VariableRow1Cells[1]).toHaveTextContent('Filter 1');

      const section1VariableRow2Cells = within(
        section1VariableRows[2],
      ).getAllByRole('cell');

      expect(section1VariableRow2Cells[0]).toHaveTextContent('indicator_1');
      expect(section1VariableRow2Cells[1]).toHaveTextContent('Indicator 1');

      // Subject 2

      const subject2 = within(subjects[1]);

      expect(subject2.getByTestId('Filename')).toHaveTextContent(
        'subject-2.csv',
      );
      expect(subject2.getByTestId('Geographic levels')).toHaveTextContent(
        'Regional; Ward',
      );
      expect(subject2.getByTestId('Time period')).toHaveTextContent(
        '2020 to 2021',
      );

      expect(subject2.getByLabelText('File guidance content')).toHaveValue(
        '<p>Test subject 2 content</p>',
      );

      userEvent.click(
        subject2.getByRole('button', {
          name: 'Variable names and descriptions',
        }),
      );

      const section2VariableRows = subject2.getAllByRole('row');

      const section2VariableRow1Cells = within(
        section2VariableRows[1],
      ).getAllByRole('cell');

      expect(section2VariableRow1Cells[0]).toHaveTextContent('filter_2');
      expect(section2VariableRow1Cells[1]).toHaveTextContent('Filter 2');

      const section2VariableRow2Cells = within(
        section2VariableRows[2],
      ).getAllByRole('cell');

      expect(section2VariableRow2Cells[0]).toHaveTextContent('indicator_2');
      expect(section2VariableRow2Cells[1]).toHaveTextContent('Indicator 2');
    });

    test('renders correctly with subjects in preview mode', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(
          screen.getByRole('button', { name: 'Preview guidance' }),
        ).toBeInTheDocument();
      });

      userEvent.click(screen.getByRole('button', { name: 'Preview guidance' }));

      expect(
        screen.queryByLabelText('Main guidance content'),
      ).not.toBeInTheDocument();

      expect(
        screen.getByTestId('mainGuidanceContent').innerHTML,
      ).toMatchInlineSnapshot(
        `
        <p>
          Test main content
        </p>
      `,
      );

      const subjects = screen.getAllByTestId('accordionSection');

      // Subject 1

      const subject1 = within(subjects[0]);

      expect(subject1.getByTestId('Filename')).toHaveTextContent(
        'subject-1.csv',
      );
      expect(subject1.getByTestId('Geographic levels')).toHaveTextContent(
        'Local Authority; National',
      );
      expect(subject1.getByTestId('Time period')).toHaveTextContent(
        '2018 to 2019',
      );

      expect(
        subject1.queryByLabelText('File guidance content'),
      ).not.toBeInTheDocument();

      expect(
        subject1.getByTestId('fileGuidanceContent').innerHTML,
      ).toMatchInlineSnapshot(
        `
        <p>
          Test subject 1 content
        </p>
      `,
      );

      userEvent.click(
        subject1.getByRole('button', {
          name: 'Variable names and descriptions',
        }),
      );

      const section1VariableRows = subject1.getAllByRole('row');

      const section1VariableRow1Cells = within(
        section1VariableRows[1],
      ).getAllByRole('cell');

      expect(section1VariableRow1Cells[0]).toHaveTextContent('filter_1');
      expect(section1VariableRow1Cells[1]).toHaveTextContent('Filter 1');

      const section1VariableRow2Cells = within(
        section1VariableRows[2],
      ).getAllByRole('cell');

      expect(section1VariableRow2Cells[0]).toHaveTextContent('indicator_1');
      expect(section1VariableRow2Cells[1]).toHaveTextContent('Indicator 1');

      // Subject 2

      const subject2 = within(subjects[1]);

      expect(subject2.getByTestId('Filename')).toHaveTextContent(
        'subject-2.csv',
      );
      expect(subject2.getByTestId('Geographic levels')).toHaveTextContent(
        'Regional; Ward',
      );
      expect(subject2.getByTestId('Time period')).toHaveTextContent(
        '2020 to 2021',
      );

      expect(
        subject2.queryByLabelText('File guidance content'),
      ).not.toBeInTheDocument();

      expect(
        subject2.getByTestId('fileGuidanceContent').innerHTML,
      ).toMatchInlineSnapshot(
        `
        <p>
          Test subject 2 content
        </p>
      `,
      );

      userEvent.click(
        subject2.getByRole('button', {
          name: 'Variable names and descriptions',
        }),
      );

      const section2VariableRows = subject2.getAllByRole('row');

      const section2VariableRow1Cells = within(
        section2VariableRows[1],
      ).getAllByRole('cell');

      expect(section2VariableRow1Cells[0]).toHaveTextContent('filter_2');
      expect(section2VariableRow1Cells[1]).toHaveTextContent('Filter 2');

      const section2VariableRow2Cells = within(
        section2VariableRows[2],
      ).getAllByRole('cell');

      expect(section2VariableRow2Cells[0]).toHaveTextContent('indicator_2');
      expect(section2VariableRow2Cells[1]).toHaveTextContent('Indicator 2');
    });

    test('shows validation message when main guidance content is empty', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(
          screen.getByLabelText('Main guidance content'),
        ).toBeInTheDocument();
      });

      const mainGuidanceContent = screen.getByLabelText(
        'Main guidance content',
      );

      expect(
        screen.queryByRole('link', { name: 'Enter main guidance content' }),
      ).not.toBeInTheDocument();

      userEvent.clear(mainGuidanceContent);
      userEvent.tab();

      await waitFor(() => {
        expect(
          screen.getByRole('link', { name: 'Enter main guidance content' }),
        ).toHaveAttribute('href', '#metaGuidanceForm-content');

        expect(mainGuidanceContent).toHaveAttribute(
          'id',
          'metaGuidanceForm-content',
        );
      });
    });

    test('shows validation message when file guidance content is empty', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(screen.getAllByTestId('accordionSection')).toHaveLength(2);
      });

      const subjects = screen.getAllByTestId('accordionSection');
      const subject1 = within(subjects[0]);

      const fileGuidanceContent = subject1.getByLabelText(
        'File guidance content',
      );

      expect(
        screen.queryByRole('link', {
          name: 'Enter file guidance content for Subject 1',
        }),
      ).not.toBeInTheDocument();

      userEvent.clear(fileGuidanceContent);
      userEvent.tab();

      await waitFor(() => {
        expect(
          screen.getByRole('link', {
            name: 'Enter file guidance content for Subject 1',
          }),
        ).toHaveAttribute('href', '#metaGuidanceForm-subjects0Content');

        expect(fileGuidanceContent).toHaveAttribute(
          'id',
          'metaGuidanceForm-subjects-0-content',
        );
      });
    });

    test('cannot submit with invalid main guidance content', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(
          screen.getByLabelText('Main guidance content'),
        ).toBeInTheDocument();
      });

      userEvent.clear(screen.getByLabelText('Main guidance content'));

      expect(
        releaseMetaGuidanceService.updateMetaGuidance,
      ).not.toHaveBeenCalled();

      userEvent.click(screen.getByRole('button', { name: 'Save guidance' }));

      await waitFor(() => {
        expect(
          screen.getByRole('link', {
            name: 'Enter main guidance content',
          }),
        ).toHaveAttribute('href', '#metaGuidanceForm-content');

        expect(
          releaseMetaGuidanceService.updateMetaGuidance,
        ).not.toHaveBeenCalled();
      });
    });

    test('cannot submit with invalid subject content', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(screen.getAllByTestId('accordionSection')).toHaveLength(2);
      });

      const subjects = screen.getAllByTestId('accordionSection');

      userEvent.clear(
        within(subjects[0]).getByLabelText('File guidance content'),
      );

      expect(
        releaseMetaGuidanceService.updateMetaGuidance,
      ).not.toHaveBeenCalled();

      userEvent.click(screen.getByRole('button', { name: 'Save guidance' }));

      await waitFor(() => {
        expect(
          screen.getByRole('link', {
            name: 'Enter file guidance content for Subject 1',
          }),
        ).toHaveAttribute('href', '#metaGuidanceForm-subjects0Content');

        expect(
          releaseMetaGuidanceService.updateMetaGuidance,
        ).not.toHaveBeenCalled();
      });
    });

    test('can successfully submit with updated values', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection releaseId="release-1" canUpdateRelease />,
      );

      await waitFor(() => {
        expect(
          screen.getByLabelText('Main guidance content'),
        ).toBeInTheDocument();
      });

      userEvent.clear(screen.getByLabelText('Main guidance content'));
      await userEvent.type(
        screen.getByLabelText('Main guidance content'),
        '<p>Updated main guidance content</p>',
      );

      const subjects = screen.getAllByTestId('accordionSection');

      const subject1 = within(subjects[0]);
      const subject2 = within(subjects[1]);

      userEvent.clear(subject1.getByLabelText('File guidance content'));
      await userEvent.type(
        subject1.getByLabelText('File guidance content'),
        '<p>Updated subject 1 guidance content</p>',
      );

      userEvent.clear(subject2.getByLabelText('File guidance content'));
      await userEvent.type(
        subject2.getByLabelText('File guidance content'),
        '<p>Updated subject 2 guidance content</p>',
      );

      // Not the right return value, but we'll just
      // return something to make sure things don't break.
      releaseMetaGuidanceService.updateMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      expect(
        releaseMetaGuidanceService.updateMetaGuidance,
      ).not.toHaveBeenCalled();

      userEvent.click(screen.getByRole('button', { name: 'Save guidance' }));

      await waitFor(() => {
        expect(
          releaseMetaGuidanceService.updateMetaGuidance,
        ).toHaveBeenCalledTimes(1);
        expect(
          releaseMetaGuidanceService.updateMetaGuidance,
        ).toHaveBeenCalledWith('release-1', {
          content: '<p>Updated main guidance content</p>',
          subjects: [
            {
              id: 'subject-1',
              content: '<p>Updated subject 1 guidance content</p>',
            },
            {
              id: 'subject-2',
              content: '<p>Updated subject 2 guidance content</p>',
            },
          ],
        });
      });
    });
  });

  describe('cannot update release', () => {
    test('renders with warning message', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection
          releaseId="release-1"
          canUpdateRelease={false}
        />,
      );

      await waitFor(() => {
        expect(
          screen.getByText(
            'This release has been approved, and can no longer be updated.',
          ),
        ).toBeInTheDocument();
      });
    });

    test('does not render any edit buttons', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection
          releaseId="release-1"
          canUpdateRelease={false}
        />,
      );

      await waitFor(() => {
        expect(
          screen.queryByRole('button', {
            name: /(Save|Edit|Preview) guidance/,
          }),
        ).not.toBeInTheDocument();
      });
    });

    test('renders empty guidance correctly', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue({
        ...testMetaGuidance,
        content: '',
      });

      render(
        <ReleaseMetaGuidanceSection
          releaseId="release-1"
          canUpdateRelease={false}
        />,
      );

      await waitFor(() => {
        expect(
          screen.getByText('No guidance content was saved.'),
        ).toBeInTheDocument();
      });
    });

    test('renders message when there are no subjects', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue({
        ...testMetaGuidance,
        subjects: [],
      });

      render(
        <ReleaseMetaGuidanceSection
          releaseId="release-1"
          canUpdateRelease={false}
        />,
      );

      await waitFor(() => {
        expect(
          screen.getByText(
            'The public metadata guidance document has not been created as no data files were uploaded.',
          ),
        ).toBeInTheDocument();
      });

      expect(
        screen.queryByTestId('mainGuidanceContent'),
      ).not.toBeInTheDocument();
      expect(screen.queryByText('Data files')).not.toBeInTheDocument();
      expect(screen.queryAllByRole('accordionSection')).toHaveLength(0);
    });

    test('renders existing guidance with subjects', async () => {
      releaseMetaGuidanceService.getMetaGuidance.mockResolvedValue(
        testMetaGuidance,
      );

      render(
        <ReleaseMetaGuidanceSection
          releaseId="release-1"
          canUpdateRelease={false}
        />,
      );

      await waitFor(() => {
        expect(screen.getByTestId('mainGuidanceContent')).toBeInTheDocument();
      });

      expect(screen.getByTestId('mainGuidanceContent').innerHTML)
        .toMatchInlineSnapshot(`
              <p>
                Test main content
              </p>
          `);

      const subjects = screen.getAllByTestId('accordionSection');

      // Subject 1

      const subject1 = within(subjects[0]);

      expect(subject1.getByTestId('Filename')).toHaveTextContent(
        'subject-1.csv',
      );
      expect(subject1.getByTestId('Geographic levels')).toHaveTextContent(
        'Local Authority; National',
      );
      expect(subject1.getByTestId('Time period')).toHaveTextContent(
        '2018 to 2019',
      );

      expect(subject1.getByTestId('fileGuidanceContent').innerHTML)
        .toMatchInlineSnapshot(`
              <p>
                Test subject 1 content
              </p>
          `);

      userEvent.click(
        subject1.getByRole('button', {
          name: 'Variable names and descriptions',
        }),
      );

      const section1VariableRows = subject1.getAllByRole('row');

      const section1VariableRow1Cells = within(
        section1VariableRows[1],
      ).getAllByRole('cell');

      expect(section1VariableRow1Cells[0]).toHaveTextContent('filter_1');
      expect(section1VariableRow1Cells[1]).toHaveTextContent('Filter 1');

      const section1VariableRow2Cells = within(
        section1VariableRows[2],
      ).getAllByRole('cell');

      expect(section1VariableRow2Cells[0]).toHaveTextContent('indicator_1');
      expect(section1VariableRow2Cells[1]).toHaveTextContent('Indicator 1');

      // Subject 2

      const subject2 = within(subjects[1]);

      expect(subject2.getByTestId('Filename')).toHaveTextContent(
        'subject-2.csv',
      );
      expect(subject2.getByTestId('Geographic levels')).toHaveTextContent(
        'Regional; Ward',
      );
      expect(subject2.getByTestId('Time period')).toHaveTextContent(
        '2020 to 2021',
      );

      expect(subject2.getByTestId('fileGuidanceContent').innerHTML)
        .toMatchInlineSnapshot(`
              <p>
                Test subject 2 content
              </p>
          `);

      userEvent.click(
        subject2.getByRole('button', {
          name: 'Variable names and descriptions',
        }),
      );

      const section2VariableRows = subject2.getAllByRole('row');

      const section2VariableRow1Cells = within(
        section2VariableRows[1],
      ).getAllByRole('cell');

      expect(section2VariableRow1Cells[0]).toHaveTextContent('filter_2');
      expect(section2VariableRow1Cells[1]).toHaveTextContent('Filter 2');

      const section2VariableRow2Cells = within(
        section2VariableRows[2],
      ).getAllByRole('cell');

      expect(section2VariableRow2Cells[0]).toHaveTextContent('indicator_2');
      expect(section2VariableRow2Cells[1]).toHaveTextContent('Indicator 2');
    });
  });
});
