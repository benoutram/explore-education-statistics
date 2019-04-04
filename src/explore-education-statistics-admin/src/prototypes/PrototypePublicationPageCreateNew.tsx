import React from "react";
import Accordion from "../components/Accordion";
import AccordionSection from "../components/AccordionSection";
import Details from "../components/Details";
import Link from "../components/Link";
import PrototypeAbsenceData from "./components/PrototypeAbsenceData";
import PrototypeDataSample from "./components/PrototypeDataSample";
import PrototypeMap from "./components/PrototypeMap";
import PrototypePage from "./components/PrototypePage";
import { PrototypeEditableContent } from "./components/PrototypeEditableContent";
import TabsSection from "../components/TabsSection";

const PublicationPage = () => {
  let mapRef: PrototypeMap | null = null;

  return (
    <PrototypePage
      wide
      breadcrumbs={[
        {
          link: "/prototypes/admin-dashboard",
          text: "Administrator dashboard"
        },
        { text: "Create new release", link: "#" }
      ]}
    >
      <h1 className="govuk-heading-xl">Create new release</h1>
      <form action="/prototypes/publication-create-new-absence">
        <div className="govuk-form-group">
          <label htmlFor="title" className="govuk-label govuk-label--s">
            Publication title
          </label>
          <input
            className="govuk-input"
            id="title"
            name="title"
            type="text"
            value="Pupil absence statistics and data for schools in England"
          />
        </div>
        <div className="govuk-form-group">
          <fieldset className="govuk-fieldset">
            <legend className="govuk-fieldset__legend govuk-fieldset__legend--s">
              Release type
            </legend>
            <div className="govuk-radios">
              <div className="govuk-radios__item">
                <input
                  className="govuk-radios__input"
                  type="radio"
                  name="release-type"
                  id="release-type-academic"
                  value="academic-year"
                  defaultChecked
                />
                <label
                  className="govuk-label govuk-radios__label"
                  htmlFor="release-type-academic"
                >
                  Academic year
                </label>
              </div>
              <div className="govuk-radios__item">
                <input
                  className="govuk-radios__input"
                  type="radio"
                  name="release-type"
                  id="release-type-calendar-year"
                  value="calendar-year"
                />
                <label
                  className="govuk-label govuk-radios__label"
                  htmlFor="release-type-calendar-year"
                >
                  Calendar year
                </label>
              </div>
            </div>
          </fieldset>
        </div>
        <div className="govuk-form-group">
          <label
            htmlFor="release-period"
            className="govuk-label govuk-label--s"
          >
            Release period
          </label>
          <span className="govuk-hint">
            For academic year use format YYYY / YYYY
          </span>
          <input
            className="govuk-input govuk-input--width-10"
            id="release-period"
            name="release-period"
            type="text"
            value="2018 / 2019"
          />
        </div>
        <div className="govuk-form-group">
          <fieldset className="govuk-fieldset">
            <legend className="govuk-fieldset__legend govuk-fieldset__legend--s">
              Setup
            </legend>
            <div className="govuk-radios">
              <div className="govuk-radios__item">
                <input
                  className="govuk-radios__input"
                  type="radio"
                  name="release-setup"
                  id="release-setup-blank"
                  value="create-blank-template"
                />
                <label
                  className="govuk-label govuk-radios__label"
                  htmlFor="release-setup-blank"
                >
                  Create from blank template
                </label>
              </div>
              <div className="govuk-radios__item">
                <input
                  className="govuk-radios__input"
                  type="radio"
                  name="release-setup"
                  id="release-setup-copy-structure"
                  value="create-copy-structure"
                />
                <label
                  className="govuk-label govuk-radios__label"
                  htmlFor="release-setup-copy-structure"
                >
                  Copy structure of current release (2017 / 2018)
                </label>
              </div>
              <div className="govuk-radios__item">
                <input
                  className="govuk-radios__input"
                  type="radio"
                  name="release-setup"
                  id="release-setup-copy-data-structure"
                  value="create-copy-data-structure"
                />
                <label
                  className="govuk-label govuk-radios__label"
                  htmlFor="release-setup-copy-data-structure"
                >
                  Copy structure, content and data of current release (2017 /
                  2018)
                </label>
              </div>
            </div>
          </fieldset>
        </div>
        <button type="submit" className="govuk-button">
          Create new release
        </button>
      </form>
    </PrototypePage>
  );
};

export default PublicationPage;
