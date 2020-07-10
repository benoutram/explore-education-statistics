import React, { useState } from 'react';
import Accordion from '@common/components/Accordion';
import AccordionSection from '@common/components/AccordionSection';

const MetaVariables = () => {
  return (
    <>
      <h2 className="govuk-heading-m govuk-!-margin-top-9">Annex A: </h2>
      <Accordion id="dataFiles">
        <AccordionSection
          heading="Variable listing and descriptions"
          goToTop={false}
        >
          <p>
            Variable names and descriptions included across the underlying data
            files are provided below
          </p>
          <table className="govuk-table">
            <thead>
              <tr>
                <th scope="col">Variable name</th>
                <th scope="col">Variable description</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>time_identifier</td>
                <td>The type of time period covered</td>
              </tr>
              <tr>
                <td>year_breakdown</td>
                <td>The breakdown of the year covered</td>
              </tr>
              <tr>
                <td>time_period</td>
                <td>The year/s covered</td>
              </tr>
              <tr>
                <td>geographic_level</td>
                <td>The geographic level of the data</td>
              </tr>
              <tr>
                <td>country_code</td>
                <td>9 digit country code</td>
              </tr>
              <tr>
                <td>country_name </td>
                <td>Country name</td>
              </tr>
              <tr>
                <td>region_code</td>
                <td>Region code (Government Office Region) - 9 digit code</td>
              </tr>
              <tr>
                <td>region_name</td>
                <td>Region name (Government Office Region)</td>
              </tr>
            </tbody>
          </table>
        </AccordionSection>
      </Accordion>
    </>
  );
};

export default MetaVariables;
