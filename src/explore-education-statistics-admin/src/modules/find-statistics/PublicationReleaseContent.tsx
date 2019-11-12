import { EditableAccordionProps } from '@admin/components/EditableAccordion';
import { EditableAccordionSectionProps } from '@admin/components/EditableAccordionSection';
import { Props as LinkProps } from '@admin/components/Link';
import BasicReleaseSummary from '@admin/modules/find-statistics/components/BasicReleaseSummary';
import { MarkdownRendererProps } from '@admin/modules/find-statistics/components/EditableMarkdownRenderer';
import { TextRendererProps } from '@admin/modules/find-statistics/components/EditableTextRenderer';
import { PrintThisPageProps } from '@admin/modules/find-statistics/components/PrintThisPage';
import { getTimePeriodCoverageDateRangeStringShort } from '@admin/pages/release/util/releaseSummaryUtil';
import { BasicPublicationDetails } from '@admin/services/common/types';
import { EditableContentBlock } from '@admin/services/publicationService';
import { ReleaseSummaryDetails } from '@admin/services/release/types';
import { generateIdList } from '@common/components/Accordion';
import Details from '@common/components/Details';
import FormattedDate from '@common/components/FormattedDate';
import PageSearchForm from '@common/components/PageSearchForm';
import RelatedAside from '@common/components/RelatedAside';
import { DataBlockProps } from '@common/modules/find-statistics/components/DataBlock';
import { baseUrl } from '@common/services/api';
import {
  AbstractRelease,
  ReleaseType,
} from '@common/services/publicationService';
import { Dictionary } from '@common/types';
import classNames from 'classnames';
import React from 'react';
import { Props as ContentBlockProps } from './components/EditableContentBlock';

export interface RendererProps {
  contentId?: string;
  releaseId?: string;
}

type TextRendererType = React.ComponentType<TextRendererProps>;
type MarkdownRendererType = React.ComponentType<MarkdownRendererProps>;
type LinkType = React.ComponentType<LinkProps & { analytics?: unknown }>;
type PrintThisPageType = React.ComponentType<PrintThisPageProps>;
type DataBlockType = React.ComponentType<DataBlockProps>;
type AccordionType = React.ComponentType<EditableAccordionProps>;
type AccordionSectionType = React.ComponentType<EditableAccordionSectionProps>;
type ContentBlockType = React.ComponentType<ContentBlockProps>;

export interface ComponentTypes {
  TextRenderer: TextRendererType;
  MarkdownRenderer: MarkdownRendererType;
  SearchForm: typeof PageSearchForm;
  PrintThisPage: PrintThisPageType;
  Link: LinkType;
  DataBlock: DataBlockType;
  Accordion: AccordionType;
  AccordionSection: AccordionSectionType;
  ContentBlock: ContentBlockType;
}

interface Props extends ComponentTypes {
  basicPublication: BasicPublicationDetails;
  release: AbstractRelease<EditableContentBlock>;
  releaseSummary: ReleaseSummaryDetails;
  styles: Dictionary<string>;

  logEvent?: (...params: string[]) => void;
}

const nullLogEvent = () => {};

const PublicationReleaseContent = ({
  basicPublication,
  release,
  releaseSummary,
  styles,
  TextRenderer,
  MarkdownRenderer,
  SearchForm,
  PrintThisPage,
  Link,
  DataBlock,
  Accordion,
  AccordionSection,
  ContentBlock,
  logEvent = nullLogEvent,
}: Props) => {
  const accId: string[] = generateIdList(2);

  const releaseCount =
    release.publication.releases.length +
    release.publication.legacyReleases.length;
  const { publication } = release;

  return (
    <>
      <h1 className="govuk-heading-l">
        <span className="govuk-caption-l">
          {releaseSummary.timePeriodCoverage.label}{' '}
          {getTimePeriodCoverageDateRangeStringShort(
            releaseSummary.releaseName,
            '/',
          )}
        </span>
        {publication.title}
      </h1>

      <div className={classNames('govuk-grid-row', styles.releaseIntro)}>
        <div className="govuk-grid-column-two-thirds">
          <div className="govuk-grid-row">
            <BasicReleaseSummary release={releaseSummary} />
          </div>

          <MarkdownRenderer
            contentId=""
            releaseId={releaseSummary.id}
            source={release.summary}
          />

          {release.downloadFiles && (
            <Details
              summary="Download data files"
              onToggle={(open: boolean) =>
                open &&
                logEvent(
                  'Downloads',
                  'Release page download data files dropdown opened',
                  window.location.pathname,
                )
              }
            >
              <ul className="govuk-list govuk-list--bullet">
                {release.downloadFiles.map(
                  ({ extension, name, path, size }) => (
                    <li key={path}>
                      <Link
                        to={`${baseUrl.data}/download/${path}`}
                        className="govuk-link"
                        analytics={{
                          category: 'Downloads',
                          action: `Release page ${name} file downloaded`,
                          label: `File URL: /api/download/${path}`,
                        }}
                      >
                        {name}
                      </Link>
                      {` (${extension}, ${size})`}
                    </li>
                  ),
                )}
              </ul>
            </Details>
          )}
          <SearchForm
            id="search-form"
            inputLabel="Search in this release page."
            className="govuk-!-margin-top-3 govuk-!-margin-bottom-3"
          />
        </div>

        <div className="govuk-grid-column-one-third">
          <PrintThisPage
            analytics={{
              category: 'Page print',
              action: 'Print this page link selected',
            }}
          />
          <RelatedAside>
            <h2 className="govuk-heading-m">About these statistics</h2>

            <dl className="dfe-meta-content">
              <dt className="govuk-caption-m">For {release.coverageTitle}:</dt>
              <dd data-testid="release-name">
                <strong>{release.yearTitle}</strong>
              </dd>
              <dd>
                <Details
                  summary={`See previous ${releaseCount} releases`}
                  onToggle={(open: boolean) =>
                    open &&
                    logEvent(
                      'Previous Releases',
                      'Release page previous releases dropdown opened',
                      window.location.pathname,
                    )
                  }
                >
                  <ul className="govuk-list">
                    {[
                      ...release.publication.releases.map(
                        ({ id, slug, releaseName }) => [
                          releaseName,
                          <li key={id} data-testid="previous-release-item">
                            <Link
                              to={`/find-statistics/${release.publication.slug}/${slug}`}
                            >
                              {releaseName}
                            </Link>
                          </li>,
                        ],
                      ),
                      ...release.publication.legacyReleases.map(
                        ({ id, description, url }) => [
                          description,
                          <li key={id} data-testid="previous-release-item">
                            <a href={url}>{description}</a>
                          </li>,
                        ],
                      ),
                    ]
                      .sort((a, b) =>
                        b[0].toString().localeCompare(a[0].toString()),
                      )
                      .map(items => items[1])}
                  </ul>
                </Details>
              </dd>
            </dl>
            <dl className="dfe-meta-content">
              <dt className="govuk-caption-m">Last updated:</dt>
              <dd data-testid="last-updated">
                <strong>
                  <FormattedDate>{release.updates[0].on}</FormattedDate>
                </strong>
                <Details
                  onToggle={(open: boolean) =>
                    open &&
                    logEvent(
                      'Last Updates',
                      'Release page last updates dropdown opened',
                      window.location.pathname,
                    )
                  }
                  summary={`See all ${release.updates.length} updates`}
                >
                  {release.updates.map(elem => (
                    <div data-testid="last-updated-element" key={elem.on}>
                      <FormattedDate className="govuk-body govuk-!-font-weight-bold">
                        {elem.on}
                      </FormattedDate>
                      <p>{elem.reason}</p>
                    </div>
                  ))}
                </Details>
              </dd>
            </dl>
            <h2
              className="govuk-heading-m govuk-!-margin-top-6"
              id="related-content"
            >
              Related guidance
            </h2>
            <nav role="navigation" aria-labelledby="related-content">
              <ul className="govuk-list">
                <li>
                  <Link to={`/methodology/${release.publication.slug}`}>
                    {`${release.publication.title}: methodology`}
                  </Link>
                </li>
              </ul>
            </nav>
          </RelatedAside>
        </div>
      </div>

      <hr />

      <h2 className="dfe-print-break-before">
        Headline facts and figures - {release.yearTitle}
      </h2>

      <DataBlock {...release.keyStatistics} id="keystats" />

      {/* <editor-fold desc="Content blocks"> */}

      {release.content.length > 0 && (
        <Accordion id={accId[0]}>
          {release.content.map(
            ({ heading, caption, order, content }, index) => {
              return (
                <AccordionSection
                  index={index}
                  heading={heading}
                  caption={caption}
                  key={order}
                >
                  <ContentBlock
                    content={content}
                    id={`content_${order}`}
                    publication={release.publication}
                  />
                </AccordionSection>
              );
            },
          )}
        </Accordion>
      )}

      {/* </editor-fold> */}

      {/* <editor-fold desc="Help and support"> */}
      <h2
        className="govuk-heading-m govuk-!-margin-top-9"
        data-testid="extra-information"
      >
        Help and support
      </h2>

      <Accordion
        // publicationTitle={publication.title}
        id="static-content-section"
      >
        <AccordionSection
          heading={`${publication.title}: methodology`}
          caption="Find out how and why we collect, process and publish these statistics"
          headingTag="h3"
        >
          <p>
            Read our{' '}
            <Link to={`/methodology/${basicPublication.methodologyId}`}>
              {`${publication.title}: methodology`}
            </Link>{' '}
            guidance.
          </p>
        </AccordionSection>
        {releaseSummary.type &&
          releaseSummary.type.title === ReleaseType.NationalStatistics && (
            <AccordionSection heading="National Statistics" headingTag="h3">
              <p className="govuk-body">
                The{' '}
                <a href="https://www.statisticsauthority.gov.uk/">
                  United Kingdom Statistics Authority
                </a>{' '}
                designated these statistics as National Statistics in accordance
                with the{' '}
                <a href="https://www.legislation.gov.uk/ukpga/2007/18/contents">
                  Statistics and Registration Service Act 2007
                </a>{' '}
                and signifying compliance with the Code of Practice for
                Statistics.
              </p>
              <p className="govuk-body">
                Designation signifying their compliance with the authority's{' '}
                <a href="https://www.statisticsauthority.gov.uk/code-of-practice/the-code/">
                  Code of Practice for Statistics
                </a>{' '}
                which broadly means these statistics are:
              </p>
              <ul className="govuk-list govuk-list--bullet">
                <li>
                  managed impartially and objectively in the public interest
                </li>
                <li>meet identified user needs</li>
                <li>produced according to sound methods</li>
                <li>well explained and readily accessible</li>
              </ul>
              <p className="govuk-body">
                Once designated as National Statistics it's a statutory
                requirement for statistics to follow and comply with the Code of
                Practice for Statistics to be observed.
              </p>
              <p className="govuk-body">
                Find out more about the standards we follow to produce these
                statistics through our{' '}
                <a href="https://www.gov.uk/government/publications/standards-for-official-statistics-published-by-the-department-for-education">
                  Standards for official statistics published by DfE
                </a>{' '}
                guidance.
              </p>
            </AccordionSection>
          )}
        <AccordionSection heading="Contact us" headingTag="h3">
          <p>
            If you have a specific enquiry about {publication.topic.theme.title}{' '}
            statistics and data:
          </p>
          <h4 className="govuk-heading-s govuk-!-margin-bottom-0">
            {publication.contact && publication.contact.teamName}
          </h4>
          <p className="govuk-!-margin-top-0">
            Email <br />
            {publication.contact && (
              <a href={`mailto:${publication.contact.teamEmail}`}>
                {publication.contact.teamEmail}
              </a>
            )}
          </p>
          <p>
            {publication.contact && (
              <>
                Telephone: {publication.contact.contactName} <br />{' '}
                {publication.contact.contactTelNo}
              </>
            )}
          </p>
          <h4 className="govuk-heading-s govuk-!-margin-bottom-0">
            Press office
          </h4>
          <p className="govuk-!-margin-top-0">If you have a media enquiry:</p>
          <p>
            Telephone <br />
            020 7925 6789
          </p>
          <h4 className="govuk-heading-s govuk-!-margin-bottom-0">
            Public enquiries
          </h4>
          <p className="govuk-!-margin-top-0">
            If you have a general enquiry about the Department for Education
            (DfE) or education:
          </p>
          <p>
            Telephone <br />
            037 0000 2288
          </p>
        </AccordionSection>
      </Accordion>
      {/* </editor-fold> */}
    </>
  );
};

export default PublicationReleaseContent;
