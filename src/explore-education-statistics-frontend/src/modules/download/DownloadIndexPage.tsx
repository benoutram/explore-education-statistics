import Accordion, { generateIdList } from '@common/components/Accordion';
import AccordionSection from '@common/components/AccordionSection';
import Details from '@common/components/Details';
import PageSearchFormWithAnalytics from '@frontend/components/PageSearchFormWithAnalytics';
import RelatedInformation from '@common/components/RelatedInformation';
import { contentApi } from '@common/services/api';
import Link from '@frontend/components/Link';
import Page from '@frontend/components/Page';
import React, { Component } from 'react';
import PublicationDownloadList from './components/PublicationDownloadList';
import { Topic } from './components/TopicList';

interface Props {
  themes: {
    id: string;
    slug: string;
    title: string;
    summary: string;
    topics: Topic[];
  }[];
}

class DownloadIndexPage extends Component<Props> {
  private accId: string[] = generateIdList(1);

  public static defaultProps = {
    themes: [],
  };

  public static async getInitialProps() {
    const themes = await contentApi.get('/Download/tree');
    return { themes };
  }

  public render() {
    const { themes } = this.props;
    return (
      <Page title="Download latest data files" breadcrumbLabel="Download">
        <div className="govuk-grid-row">
          <div className="govuk-grid-column-two-thirds">
            <p className="govuk-body-l">
              Find the latest data files behind our range of national and
              regional statistics for your own analysis.
            </p>
            <p className="govuk-body">
              Previous release data can be found on their respective release
              pages.
            </p>
            <PageSearchFormWithAnalytics
              inputLabel="Search the latest data files behind our range of national and
              regional statistics for your own analysis."
            />
          </div>
          <div className="govuk-grid-column-one-third">
            <RelatedInformation>
              <ul className="govuk-list">
                <li>
                  <Link to="/find-statistics">Find statistics and data</Link>
                </li>
                <li>
                  <Link to="/glossary">Education statistics: glossary</Link>
                </li>
                <li>
                  <Link to="/methodology">
                    Education statistics: methodology
                  </Link>
                </li>
              </ul>
            </RelatedInformation>
          </div>
        </div>

        {themes.length > 0 ? (
          <Accordion id={this.accId[0]}>
            {themes.map(
              ({
                id: themeId,
                title: themeTitle,
                summary: themeSummary,
                topics,
              }) => (
                <AccordionSection
                  key={themeId}
                  heading={themeTitle}
                  caption={themeSummary}
                >
                  {topics.map(
                    ({ id: topicId, title: topicTitle, publications }) => (
                      <Details key={topicId} summary={topicTitle}>
                        <PublicationDownloadList publications={publications} />
                      </Details>
                    ),
                  )}
                </AccordionSection>
              ),
            )}
          </Accordion>
        ) : (
          <div className="govuk-inset-text">No data currently published.</div>
        )}
      </Page>
    );
  }
}

export default DownloadIndexPage;
