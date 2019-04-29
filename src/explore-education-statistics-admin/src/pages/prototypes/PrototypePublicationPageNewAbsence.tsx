import React, { Component } from 'react';
import PrototypePage from './components/PrototypePage';
import PrototypeAdminNavigation from './components/PrototypeAdminNavigation';
import { PrototypePublicationService } from '@admin/pages/prototypes/components/PrototypePublicationService';
import { EditablePublicationPage } from '@admin/pages/prototypes/components/EditablePublicationPage';

interface State {
  editing: boolean;
}

class PublicationPage extends Component<{}, State> {
  public state = {
    editing: false,
  };

  public render() {
    return (
      <PrototypePage
        wide
        breadcrumbs={[
          {
            link: '/prototypes/admin-dashboard?status=editLiveRelease',
            text: 'Administrator dashboard',
          },
          { text: 'Edit pupil absence statistics', link: '#' },
        ]}
      >
        <PrototypeAdminNavigation sectionId="addContent" task="editRelease" />

        <div className="govuk-width-container govuk-form-group">
          <fieldset className="govuk-fieldset">
            <legend className="govuk-fieldset__legend govuk-fieldset__legend--m">
              <h1 className="govuk-fieldset__heading">Set page view</h1>
            </legend>
            <div className="govuk-radios govuk-radios--inline">
              <div className="govuk-radios__item">
                <input
                  className="govuk-radios__input"
                  type="radio"
                  name="status"
                  id="edit"
                  value="edit"
                  defaultChecked
                  onChange={() => this.setState({ editing: false })}
                />
                <label
                  className="govuk-label govuk-radios__label"
                  htmlFor="edit"
                >
                  Preview content
                </label>
              </div>

              <div className="govuk-radios__item">
                <input
                  className="govuk-radios__input"
                  type="radio"
                  name="status"
                  id="edit"
                  value="edit"
                  onChange={() => this.setState({ editing: true })}
                />
                <label
                  className="govuk-label govuk-radios__label"
                  htmlFor="edit"
                >
                  Edit content
                </label>
              </div>
            </div>
          </fieldset>
        </div>

        <hr />
        <div className="govuk-width-container">
          <EditablePublicationPage
            editing={this.state.editing}
            data={PrototypePublicationService.getNewPublication()}
          />
        </div>
      </PrototypePage>
    );
  }
}

export default PublicationPage;
