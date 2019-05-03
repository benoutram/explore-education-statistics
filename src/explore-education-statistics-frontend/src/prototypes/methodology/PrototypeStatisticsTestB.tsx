import Accordion from '@common/components/Accordion';
import AccordionSection from '@common/components/AccordionSection';
import Link from '@frontend/components/Link';
import PrototypePage from '@frontend/prototypes/components/PrototypePage';
import React from 'react';

const BrowseReleasesPage = () => {
  return (
    <PrototypePage
      breadcrumbs={[
        {
          link: '/prototypes/methodology-home',
          text: 'Methodology',
        },
      ]}
    >
      <h1 className="govuk-heading-xl">Find statistics and download data</h1>
      <div className="govuk-grid-row">
        <div className="govuk-grid-column-two-thirds">
          <p className="govuk-body-l">
            Browse to find the statistics and data you’re looking for and open
            the section to get links to:
          </p>
          <ul className="govuk-bulllet-list govuk-!-margin-bottom-9">
            <li>
              up-to-date national statistical headlines, breakdowns and
              explanations
            </li>
            <li>
              charts and tables to help you compare, contrast and view national
              and regional statistical data and trends
            </li>
            <li>
              our table tool to build your own tables online and explore our
              range of national and regional data
            </li>
            <li>
              links to underlying data so you can download files and carry out
              your own statistical analysis
            </li>
          </ul>
        </div>
        <div className="govuk-grid-column-one-third">
          <aside className="app-related-items">
            <h2 className="govuk-heading-m" id="releated-content">
              Related content
            </h2>
            <nav role="navigation" aria-labelledby="subsection-title">
              <ul className="govuk-list">
                <li>
                  <Link to="/prototypes/methodology-home">
                    Education statistics: methodology
                  </Link>
                </li>
                <li>
                  <Link to="https://eesadminprototype.z33.web.core.windows.net/prototypes/documentation/glossary">
                    Education statistics: glossary
                  </Link>
                </li>
              </ul>
            </nav>
          </aside>
        </div>
      </div>

      <Accordion id="children-and-early-years">
        <AccordionSection heading="Children and early years - including social care">
          <h3>Childcare and early years</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Children in need and child protection</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Early years foundation stage profile</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Looked-after children</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Secure children's homes</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Special educational needs (SEN)</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
        </AccordionSection>
        <AccordionSection heading="Finance and funding">
          <h3>Advanced learner loans </h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Local authority and school finance </h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Student loan forecasts </h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
        </AccordionSection>
        <AccordionSection heading="Further education">
          <h3>Education and training</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>FE Choices</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Further education and skills</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Further education for benefits claimants</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>National achievement rates tables</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>
            Not in education, employment or training (NEET) and participation
          </h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
        </AccordionSection>
        <AccordionSection heading="Higher education">
          <h3>Education and training</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Higher education graduate employment and earnings</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Higher education statistics</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>
            Not in education, employment or training (NEET) and participation
          </h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Participation rates in higher education</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Widening participation in higher education</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
        </AccordionSection>
        <AccordionSection heading="Performance - including GCSE and key stage results">
          <h3>16 to 19 attainment</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Destinations of key stage 4 and key stage 5 pupils</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>GCSEs (key stage 4) and equivalent results</h3>
          <div className="govuk-!-margin-top-0 govuk-!-padding-top-0">
            <p className="govuk-body">
              View statistics, create charts and tables and download data files
              for GCSE and equivalent results in England
            </p>
            <div className="govuk-!-margin-top-0" />
            <p>
              <strong>NEED TO GET DROPDOWN INSERED IN HERE</strong>
            </p>
          </div>
          <h3>Key stage 1</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Key stage 2</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Outcome based success measures</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Parental responsibility measures</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Performance tables</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Widening participation in higher education</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
        </AccordionSection>
        <AccordionSection heading="Pupils and schools">
          <h3>Admission appeals</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Exclusions</h3>
          <div className="govuk-!-margin-top-0 govuk-!-padding-top-0">
            <p className="govuk-body">
              View statistics, create charts and tables and download data files
              for GCSE and equivalent results in England
            </p>
            <div className="govuk-!-margin-top-0" />
            <p>
              <strong>NEED TO GET DROPDOWN INSERED IN HERE</strong>
            </p>
          </div>
          <h3>
            Not in education, employment or training (NEET) and participation
          </h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Pupil absence</h3>
          <div className="govuk-!-margin-top-0 govuk-!-padding-top-0">
            <p className="govuk-body">
              View statistics, create charts and tables and download data files
              for GCSE and equivalent results in England
            </p>
            <div className="govuk-!-margin-top-0" />
            <p>
              <strong>NEED TO GET DROPDOWN INSERED IN HERE</strong>
            </p>
          </div>
          <h3>Pupil projections</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>School and pupil numbers</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>School applications</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>School capacity</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>School workforce</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
          <h3>Special educational needs (SEN)</h3>
          <div className="govuk-inset-text">
            These statistics and data are not yet available on the explore
            education statistics service. To find and download these statistics
            and data browse{' '}
            <a href="https://www.gov.uk/government/organisations/department-for-education/about/statistics#statistical-collections">
              Statistics at DfE
            </a>
          </div>
        </AccordionSection>
      </Accordion>
    </PrototypePage>
  );
};

export default BrowseReleasesPage;
