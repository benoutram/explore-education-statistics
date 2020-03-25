import Page from '@admin/components/Page';
import { useAuthContext } from '@admin/contexts/AuthContext';
import loginService from '@admin/services/sign-in/service';
import React from 'react';

const ForbiddenPage = () => {
  const { user } = useAuthContext();

  return (
    <Page>
      <h1 className="govuk-heading-l">Forbidden</h1>
      <p className="govuk-body">
        You do not have permission to access this page.
      </p>
      {!user && (
        <>
          <p className="govuk-body">Log in and try again.</p>
          <a
            href={loginService.getSignInLink()}
            className="govuk-button govuk-button--start"
          >
            Sign-in
          </a>
        </>
      )}
    </Page>
  );
};

export default ForbiddenPage;
