import ReleaseDataBlocksPage from '@admin/pages/release/datablocks/ReleaseDataBlocksPage';
import {
  releaseDataBlocksRoute,
  ReleaseRouteParams,
} from '@admin/routes/releaseRoutes';
import _dataBlockService, {
  DeleteDataBlockPlan,
  ReleaseDataBlockSummary,
} from '@admin/services/dataBlockService';
import _permissionService from '@admin/services/permissionService';
import { waitFor } from '@testing-library/dom';
import { render, screen, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import React from 'react';
import { generatePath, MemoryRouter } from 'react-router';
import { Route } from 'react-router-dom';

jest.mock('@admin/services/dataBlockService');
jest.mock('@admin/services/permissionService');

const dataBlockService = _dataBlockService as jest.Mocked<
  typeof _dataBlockService
>;
const permissionService = _permissionService as jest.Mocked<
  typeof _permissionService
>;

describe('ReleaseDataBlocksPage', () => {
  const testDataBlocks: ReleaseDataBlockSummary[] = [
    {
      id: 'block-1',
      name: 'Block 1',
      highlightName: 'Block 1 highlight name',
      heading: 'Block 1 heading',
      source: 'Block 1 source',
      contentSectionId: 'section-1',
      chartsCount: 1,
    },
    {
      id: 'block-2',
      name: 'Block 2',
      heading: 'Block 2 heading',
      source: 'Block 2 source',
      contentSectionId: '',
      chartsCount: 0,
    },
  ];

  const testBlock1DeletePlan: DeleteDataBlockPlan = {
    dependentDataBlocks: [
      {
        name: 'Block 1',
        contentSectionHeading: 'Section 1',
        infographicFilesInfo: [],
      },
    ],
  };

  beforeEach(() => {
    permissionService.canUpdateRelease.mockResolvedValue(true);
  });

  test('renders list of data blocks correctly', async () => {
    dataBlockService.listDataBlocks.mockResolvedValue(testDataBlocks);

    renderPage();

    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    const rows = screen.getAllByRole('row');

    expect(rows).toHaveLength(3);

    const row1Cells = within(rows[1]).getAllByRole('cell');

    expect(row1Cells).toHaveLength(5);
    expect(row1Cells[0]).toHaveTextContent('Block 1');
    expect(row1Cells[1]).toHaveTextContent('Yes');
    expect(row1Cells[2]).toHaveTextContent('Yes');
    expect(row1Cells[3]).toHaveTextContent('Block 1 highlight name');
    expect(within(row1Cells[4]).getByRole('link')).toHaveAttribute(
      'href',
      '/publication/publication-1/release/release-1/data-blocks/block-1',
    );

    const row2Cells = within(rows[2]).getAllByRole('cell');

    expect(row2Cells).toHaveLength(5);
    expect(row2Cells[0]).toHaveTextContent('Block 2');
    expect(row2Cells[1]).toHaveTextContent('No');
    expect(row2Cells[2]).toHaveTextContent('No');
    expect(row2Cells[3]).toHaveTextContent('None');
    expect(within(row2Cells[4]).getByRole('link')).toHaveAttribute(
      'href',
      '/publication/publication-1/release/release-1/data-blocks/block-2',
    );
  });

  test('renders page correctly when release cannot be updated', async () => {
    permissionService.canUpdateRelease.mockResolvedValue(false);

    dataBlockService.listDataBlocks.mockResolvedValue(testDataBlocks);

    renderPage();

    await waitFor(() => {
      expect(
        screen.getByText(
          /This release has been approved, and can no longer be updated/,
        ),
      ).toBeInTheDocument();

      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    const rows = screen.getAllByRole('row');

    expect(rows).toHaveLength(3);

    const row1Cells = within(rows[1]).getAllByRole('cell');

    expect(row1Cells).toHaveLength(4);
    expect(row1Cells[0]).toHaveTextContent('Block 1');
    expect(row1Cells[1]).toHaveTextContent('Yes');
    expect(row1Cells[2]).toHaveTextContent('Yes');
    expect(row1Cells[3]).toHaveTextContent('Block 1 highlight name');

    const row2Cells = within(rows[2]).getAllByRole('cell');

    expect(row2Cells).toHaveLength(4);
    expect(row2Cells[0]).toHaveTextContent('Block 2');
    expect(row2Cells[1]).toHaveTextContent('No');
    expect(row2Cells[2]).toHaveTextContent('No');
    expect(row2Cells[3]).toHaveTextContent('None');

    expect(
      screen.queryByRole('button', { name: 'Create data block' }),
    ).not.toBeInTheDocument();
  });

  test('clicking `Delete block` button shows modal', async () => {
    dataBlockService.listDataBlocks.mockResolvedValue(testDataBlocks);
    dataBlockService.getDeleteBlockPlan.mockResolvedValue(testBlock1DeletePlan);

    renderPage();

    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    expect(screen.queryByRole('dialog')).not.toBeInTheDocument();
    expect(dataBlockService.getDeleteBlockPlan).toHaveBeenCalledTimes(0);

    const buttons = screen.getAllByRole('button', { name: 'Delete block' });

    userEvent.click(buttons[0]);

    await waitFor(() => {
      expect(screen.getByRole('dialog')).toBeInTheDocument();
    });

    expect(dataBlockService.getDeleteBlockPlan).toHaveBeenCalledTimes(1);

    const modal = within(screen.getByRole('dialog'));

    expect(modal.getByTestId('deleteDataBlock-name')).toHaveTextContent(
      'Block 1',
    );
    expect(
      modal.getByTestId('deleteDataBlock-contentSectionHeading'),
    ).toHaveTextContent('Section 1');
  });

  test('clicking `Cancel` button hides modal', async () => {
    dataBlockService.listDataBlocks.mockResolvedValue(testDataBlocks);
    dataBlockService.getDeleteBlockPlan.mockResolvedValue(testBlock1DeletePlan);

    renderPage();

    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    const buttons = screen.getAllByRole('button', { name: 'Delete block' });

    userEvent.click(buttons[0]);

    await waitFor(() => {
      expect(screen.getByRole('dialog')).toBeInTheDocument();
    });

    const modal = within(screen.getByRole('dialog'));

    userEvent.click(modal.getByRole('button', { name: 'Cancel' }));

    await waitFor(() => {
      expect(screen.queryByRole('dialog')).not.toBeInTheDocument();
    });
  });

  test('clicking `Confirm` button hides modal and deletes data block', async () => {
    dataBlockService.listDataBlocks.mockResolvedValue(testDataBlocks);
    dataBlockService.getDeleteBlockPlan.mockResolvedValue(testBlock1DeletePlan);

    renderPage();

    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    const buttons = screen.getAllByRole('button', { name: 'Delete block' });

    userEvent.click(buttons[0]);

    await waitFor(() => {
      expect(screen.getByRole('dialog')).toBeInTheDocument();
    });

    const modal = within(screen.getByRole('dialog'));

    userEvent.click(modal.getByRole('button', { name: 'Confirm' }));

    await waitFor(() => {
      expect(screen.queryByRole('dialog')).not.toBeInTheDocument();
    });

    expect(dataBlockService.deleteDataBlock).toHaveBeenCalledTimes(1);
    expect(dataBlockService.deleteDataBlock).toHaveBeenCalledWith(
      'release-1',
      'block-1',
    );

    const rows = screen.getAllByRole('row');

    expect(rows).toHaveLength(2);

    const row1Cells = within(rows[1]).getAllByRole('cell');

    expect(row1Cells[0]).toHaveTextContent('Block 2');
    expect(row1Cells[1]).toHaveTextContent('No');
    expect(row1Cells[2]).toHaveTextContent('No');
    expect(row1Cells[3]).toHaveTextContent('None');
    expect(within(row1Cells[4]).getByRole('link')).toHaveAttribute(
      'href',
      '/publication/publication-1/release/release-1/data-blocks/block-2',
    );
  });

  const renderPage = () => {
    return render(
      <MemoryRouter
        initialEntries={[
          generatePath<ReleaseRouteParams>(releaseDataBlocksRoute.path, {
            releaseId: 'release-1',
            publicationId: 'publication-1',
          }),
        ]}
      >
        <Route
          path={releaseDataBlocksRoute.path}
          component={ReleaseDataBlocksPage}
        />
      </MemoryRouter>,
    );
  };
});
