import { Release } from '@common/services/publicationService';
import React, { Component } from 'react';
import { FormGroup, FormRadioGroup } from '@common/components/form';
import PrototypePublicationService from '@admin/pages/prototypes/components/PrototypePublicationService';
import EditablePublicationPage from '@admin/pages/prototypes/components/EditablePublicationPage';
import PrototypePage from './components/PrototypePage';
import Link from '../../components/Link';

const PublicationPage = () => {
  const [status, setStatus] = React.useState('');

  return (
    <PrototypePage
      wide
      breadcrumbs={[
        {
          link: '/prototypes/admin-dashboard?status=readyApproval',
          text: 'Administrator dashboard',
        },
        { text: 'Review release', link: '#' },
      ]}
    >
      {' '}
      <div>
        <FormRadioGroup
          legend="Release status"
          id="review-release"
          name="review-release"
          value={status}
          onChange={e => {
            setStatus(e.target.value);
          }}
          options={[
            {
              id: 'approve-release',
              label: 'Approve for higher review',
              value: 'approve-release',
              conditional: (
                <FormGroup>
                  <label htmlFor="approved-comments" className="govuk-label">
                    Add any extra comments
                  </label>
                  <textarea
                    name="approved-comments"
                    id="approved-comments"
                    className="govuk-textarea"
                  />
                </FormGroup>
              ),
            },

            {
              id: 'question',
              label: 'In review - add comments and questions',
              value: 'question',
              conditional: (
                <FormGroup>
                  <label htmlFor="question" className="govuk-label">
                    Add your comment or question
                  </label>
                  <textarea
                    name="question"
                    id="question"
                    className="govuk-textarea"
                  />
                </FormGroup>
              ),
            },
          ]}
        />

        <Link
          to="/prototypes/admin-dashboard?status=readyApproval"
          className="govuk-button"
        >
          Update
        </Link>
      </div>
      <hr />
      <div className="govuk-width-container dfe-align--comments">
        <EditablePublicationPage
          reviewing
          data={PrototypePublicationService.getNewPublication()}
        />
      </div>
    </PrototypePage>
  );
};

export default PublicationPage;
