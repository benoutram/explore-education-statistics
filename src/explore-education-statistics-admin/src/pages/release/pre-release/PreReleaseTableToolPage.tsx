import Link from '@admin/components/Link';
import PageTitle from '@admin/components/PageTitle';
import PreReleaseTableToolFinalStep from '@admin/pages/release/pre-release/components/PreReleaseTableToolFinalStep';
import {
  preReleaseTableToolRoute,
  PreReleaseTableToolRouteParams,
} from '@admin/routes/preReleaseRoutes';
import dataBlockService from '@admin/services/dataBlockService';
import publicationService from '@admin/services/publicationService';
import LoadingSpinner from '@common/components/LoadingSpinner';
import useAsyncHandledRetry from '@common/hooks/useAsyncHandledRetry';
import TableToolWizard, {
  InitialTableToolState,
} from '@common/modules/table-tool/components/TableToolWizard';
import WizardStep from '@common/modules/table-tool/components/WizardStep';
import WizardStepHeading from '@common/modules/table-tool/components/WizardStepHeading';
import mapFullTable from '@common/modules/table-tool/utils/mapFullTable';
import mapTableHeadersConfig from '@common/modules/table-tool/utils/mapTableHeadersConfig';
import tableBuilderService from '@common/services/tableBuilderService';
import React from 'react';
import { RouteComponentProps } from 'react-router';
import { generatePath } from 'react-router-dom';

const PreReleaseTableToolPage = ({
  match,
}: RouteComponentProps<PreReleaseTableToolRouteParams>) => {
  const { publicationId, releaseId, dataBlockId } = match.params;

  const { value: publication } = useAsyncHandledRetry(
    () => publicationService.getPublication(publicationId),
    [publicationId],
  );

  const { value: tableToolState, isLoading } = useAsyncHandledRetry<
    InitialTableToolState | undefined
  >(async () => {
    const { subjects, ...release } = await tableBuilderService.getRelease(
      releaseId,
    );

    const highlights = release.highlights.filter(
      highlight => highlight.id !== dataBlockId,
    );

    if (dataBlockId) {
      const { table, query } = await dataBlockService.getDataBlock(dataBlockId);

      const [subjectMeta, tableData] = await Promise.all([
        tableBuilderService.getSubjectMeta(query.subjectId),
        tableBuilderService.getTableData({
          releaseId,
          ...query,
        }),
      ]);

      const fullTable = mapFullTable(tableData);
      const tableHeaders = mapTableHeadersConfig(
        table.tableHeaders,
        fullTable.subjectMeta,
      );

      return {
        initialStep: 5,
        subjects,
        highlights,
        query: {
          ...query,
          publicationId,
          releaseId,
        },
        subjectMeta,
        response: {
          table: fullTable,
          tableHeaders,
        },
      };
    }

    return {
      initialStep: 1,
      subjects,
      highlights,
      query: {
        publicationId,
        releaseId,
        subjectId: '',
        indicators: [],
        filters: [],
        locations: {},
      },
    };
  }, [publicationId, releaseId, dataBlockId]);

  return (
    <>
      <PageTitle title="Create your own tables online" caption="Table Tool" />

      <p>
        Choose the data and area of interest you want to explore and then use
        filters to create your table.
      </p>

      <p>
        Once you've created your table, you can download the data it contains
        for your own offline analysis.
      </p>

      <LoadingSpinner loading={isLoading}>
        {tableToolState && (
          <TableToolWizard
            scrollOnMount
            themeMeta={[]}
            initialState={tableToolState}
            renderHighlightLink={highlight => (
              <Link
                to={generatePath<PreReleaseTableToolRouteParams>(
                  preReleaseTableToolRoute.path,
                  {
                    publicationId,
                    releaseId,
                    dataBlockId: highlight.id,
                  },
                )}
              >
                {highlight.name}
              </Link>
            )}
            finalStep={({ response, query }) => (
              <WizardStep>
                {wizardStepProps => (
                  <>
                    <WizardStepHeading {...wizardStepProps}>
                      Explore data
                    </WizardStepHeading>

                    {response && query && (
                      <PreReleaseTableToolFinalStep
                        publication={publication}
                        releaseId={releaseId}
                        table={response.table}
                        tableHeaders={response.tableHeaders}
                      />
                    )}
                  </>
                )}
              </WizardStep>
            )}
          />
        )}
      </LoadingSpinner>
    </>
  );
};

export default PreReleaseTableToolPage;
