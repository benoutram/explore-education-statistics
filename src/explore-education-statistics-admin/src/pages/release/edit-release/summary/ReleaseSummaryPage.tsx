import Link from '@admin/components/Link';
import DummyReferenceData from '@admin/pages/DummyReferenceData';
import PublicationContext from "@admin/pages/release/PublicationContext";
import { summaryEditRoute } from '@admin/routes/edit-release/routes';
import {
  dayMonthYearIsComplete,
  dayMonthYearToDate, IdTitlePair,
} from '@admin/services/common/types';
import service from '@admin/services/release/edit-release/summary/service';
import commonService from '@admin/services/common/service';
import { ReleaseSummaryDetails } from '@admin/services/release/types';
import FormattedDate from '@common/components/FormattedDate';
import SummaryList from '@common/components/SummaryList';
import SummaryListItem from '@common/components/SummaryListItem';
import React, {useContext, useEffect, useState} from 'react';
import { RouteComponentProps } from 'react-router';
import ReleasePageTemplate from '../components/ReleasePageTemplate';

interface MatchProps {
  publicationId: string;
  releaseId: string;
}

const ReleaseSummaryPage = ({ match }: RouteComponentProps<MatchProps>) => {
  const { publicationId, releaseId } = match.params;

  const [releaseSummaryDetails, setReleaseSummaryDetails] = useState<
    ReleaseSummaryDetails
  >();

  const [releaseTypes, setReleaseTypes] = useState<
    IdTitlePair[]
    >();

  const {publication} = useContext(PublicationContext);

  useEffect(() => {
    const releaseSummaryPromise = service.getReleaseSummaryDetails(releaseId);
    const releaseTypesPromise = commonService.getReleaseTypes();
    Promise.all([releaseSummaryPromise, releaseTypesPromise]).
      then(([releaseSummaryResult, releaseTypesResult]) => {
        setReleaseSummaryDetails(releaseSummaryResult);
        setReleaseTypes(releaseTypesResult);
      })
  }, [releaseId]);

  const getSelectedTimePeriodCoverageLabel = (timePeriodCoverageCode: string) =>
    DummyReferenceData.findTimePeriodCoverageOption(timePeriodCoverageCode)
      .label;

  const getSelectedReleaseTypeTitle = (releaseTypeId: string, availableReleaseTypes: IdTitlePair[]) =>
    availableReleaseTypes.find(type => type.title === releaseTypeId) || availableReleaseTypes[0].title;

  return (
    <>
      {releaseSummaryDetails &&  releaseTypes && publication && (
        <SummaryList>
          <SummaryListItem term="Publication title">
            {publication.title}
          </SummaryListItem>
          <SummaryListItem term="Time period">
            {getSelectedTimePeriodCoverageLabel(
              releaseSummaryDetails.timePeriodCoverageCode,
            )}
          </SummaryListItem>
          <SummaryListItem term="Release period">
            <time>{releaseSummaryDetails.releaseName}</time> to{' '}
            <time>{releaseSummaryDetails.releaseName + 1}</time>
          </SummaryListItem>
          <SummaryListItem term="Lead statistician">
            {publication.contact.contactName}
          </SummaryListItem>
          <SummaryListItem term="Scheduled release">
            <FormattedDate>
              {new Date(releaseSummaryDetails.publishScheduled)}
            </FormattedDate>
          </SummaryListItem>
          <SummaryListItem term="Next release expected">
            {dayMonthYearIsComplete(
              releaseSummaryDetails.nextReleaseDate,
            ) && (
              <FormattedDate>
                {dayMonthYearToDate(
                  releaseSummaryDetails.nextReleaseDate,
                )}
              </FormattedDate>
            )}
          </SummaryListItem>
          <SummaryListItem term="Release type">
            {getSelectedReleaseTypeTitle(releaseSummaryDetails.id, releaseTypes)}
          </SummaryListItem>
          <SummaryListItem
            term=""
            actions={
              <Link to={summaryEditRoute.generateLink(publicationId, releaseId)}>
                Edit release setup details
              </Link>
            }
          />
        </SummaryList>
      )}
    </>
  );
};

export default ReleaseSummaryPage;
