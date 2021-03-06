import Link from '@admin/components/Link';
import { useEditingContext } from '@admin/contexts/EditingContext';
import { EditableRelease } from '@admin/services/releaseContentService';
import Accordion from '@common/components/Accordion';
import AccordionSection from '@common/components/AccordionSection';
import ContactUsSection from '@common/modules/find-statistics/components/ContactUsSection';
import NationalStatisticsSection from '@common/modules/find-statistics/components/NationalStatisticsSection';
import { ReleaseType } from '@common/services/publicationService';
import React from 'react';

const ReleaseHelpAndSupportSection = ({
  release,
}: {
  release: EditableRelease;
}) => {
  const { isEditing } = useEditingContext();
  const { publication } = release;
  return (
    <>
      <h2
        className="govuk-heading-m govuk-!-margin-top-9"
        data-testid="extra-information"
      >
        Help and support
      </h2>

      <Accordion id="helpAndSupport">
        <AccordionSection
          heading="Methodology"
          caption="Find out how and why we collect, process and publish these statistics"
          headingTag="h3"
        >
          {publication.methodology || publication.externalMethodology ? (
            <p>
              Read our{' '}
              {publication.methodology &&
                (isEditing ? (
                  <a>{`${publication.title}: methodology`}</a>
                ) : (
                  <Link to={`/methodologies/${publication.methodology.id}`}>
                    {`${publication.title}: methodology`}
                  </Link>
                ))}
              {!publication.methodology &&
                publication.externalMethodology &&
                (isEditing ? (
                  <a>{`${publication.title}: methodology`}</a>
                ) : (
                  <Link to={publication.externalMethodology.url}>
                    {`${publication.title}: methodology`}
                  </Link>
                ))}{' '}
              guidance.
            </p>
          ) : (
            <p>No methodology added.</p>
          )}
        </AccordionSection>
        {release.type && release.type.title === ReleaseType.NationalStatistics && (
          <AccordionSection heading="National Statistics" headingTag="h3">
            <NationalStatisticsSection />
          </AccordionSection>
        )}
        <AccordionSection heading="Contact us" headingTag="h3">
          <ContactUsSection
            publicationContact={publication.contact}
            publicationTitle={publication.title}
          />
        </AccordionSection>
      </Accordion>
    </>
  );
};

export default ReleaseHelpAndSupportSection;
