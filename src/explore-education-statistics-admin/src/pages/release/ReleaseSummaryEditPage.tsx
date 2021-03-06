import { useLastLocation } from '@admin/contexts/LastLocationContext';
import ReleaseSummaryForm, {
  ReleaseSummaryFormValues,
} from '@admin/pages/release/components/ReleaseSummaryForm';
import { useReleaseContext } from '@admin/pages/release/contexts/ReleaseContext';
import {
  ReleaseRouteParams,
  releaseSummaryRoute,
} from '@admin/routes/releaseRoutes';
import releaseService from '@admin/services/releaseService';
import useFormSubmit from '@common/hooks/useFormSubmit';
import { mapFieldErrors } from '@common/validation/serverValidations';
import LoadingSpinner from '@common/components/LoadingSpinner';
import useAsyncRetry from '@common/hooks/useAsyncRetry';
import React from 'react';
import { generatePath, RouteComponentProps, useLocation } from 'react-router';

const errorMappings = [
  mapFieldErrors<ReleaseSummaryFormValues>({
    target: 'timePeriodCoverageStartYear',
    messages: {
      SLUG_NOT_UNIQUE:
        'Choose a unique combination of time period and start year',
    },
  }),
];

const ReleaseSummaryEditPage = ({ history }: RouteComponentProps) => {
  const location = useLocation();
  const lastLocation = useLastLocation();

  const {
    releaseId,
    release: contextRelease,
    onReleaseChange,
  } = useReleaseContext();

  const { value: release, isLoading } = useAsyncRetry(
    async () =>
      lastLocation && lastLocation !== location
        ? releaseService.getRelease(releaseId)
        : contextRelease,
    [releaseId],
  );

  const handleSubmit = useFormSubmit<ReleaseSummaryFormValues>(async values => {
    if (!release) {
      throw new Error('Could not update missing release');
    }

    const nextRelease = await releaseService.updateRelease(releaseId, {
      ...release,
      timePeriodCoverage: {
        value: values.timePeriodCoverageCode,
      },
      releaseName: values.timePeriodCoverageStartYear,
      typeId: values.releaseTypeId,
    });

    onReleaseChange(nextRelease);

    history.push(
      generatePath<ReleaseRouteParams>(releaseSummaryRoute.path, {
        publicationId: release.publicationId,
        releaseId,
      }),
    );
  }, errorMappings);

  const handleCancel = () => {
    if (!release) {
      return;
    }

    history.push(
      generatePath<ReleaseRouteParams>(releaseSummaryRoute.path, {
        publicationId: release.publicationId,
        releaseId,
      }),
    );
  };

  return (
    <LoadingSpinner loading={isLoading}>
      {release && (
        <>
          <h2>Edit release summary</h2>

          <ReleaseSummaryForm<ReleaseSummaryFormValues>
            submitText="Update release summary"
            initialValues={() => ({
              timePeriodCoverageCode: release.timePeriodCoverage.value,
              timePeriodCoverageStartYear: release.releaseName.toString(),
              releaseTypeId: release.type.id,
            })}
            onSubmit={handleSubmit}
            onCancel={handleCancel}
          />
        </>
      )}
    </LoadingSpinner>
  );
};

export default ReleaseSummaryEditPage;
