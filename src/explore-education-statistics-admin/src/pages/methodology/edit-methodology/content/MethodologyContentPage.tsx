import EditablePageModeToggle from '@admin/components/editable/EditablePageModeToggle';
import { EditingContextProvider } from '@admin/contexts/EditingContext';
import PrintThisPage from '@admin/modules/find-statistics/components/PrintThisPage';
import { MethodologyRouteParams } from '@admin/routes/edit-methodology/routes';
import methodologyService from '@admin/services/methodology/methodologyService';
import permissionService from '@admin/services/permissions/permissionService';
import FormattedDate from '@common/components/FormattedDate';
import LoadingSpinner from '@common/components/LoadingSpinner';
import PageSearchForm from '@common/components/PageSearchForm';
import WarningMessage from '@common/components/WarningMessage';
import useAsyncHandledRetry from '@common/hooks/useAsyncHandledRetry';
import React from 'react';
import { RouteComponentProps } from 'react-router';
import MethodologyAccordion from './components/MethodologyAccordion';
import {
  MethodologyContextState,
  MethodologyProvider,
  useMethodologyState,
} from './context/MethodologyContext';

const MethodologyContentPageInternal = () => {
  const { methodology } = useMethodologyState();

  return (
    <EditingContextProvider>
      {({ isEditing }) => (
        <>
          <EditablePageModeToggle />

          <div className="govuk-width-container">
            <section
              className={isEditing ? 'dfe-page-editing' : 'dfe-page-preview'}
            >
              <h1
                className="govuk-heading-xl"
                data-testid={`page-title ${methodology.title}`}
              >
                {methodology.title}
              </h1>

              <div className="govuk-grid-row">
                <div className="govuk-grid-column-two-thirds">
                  <dl className="dfe-meta-content govuk-!-margin-top-0">
                    <div>
                      <dt className="govuk-caption-m">Published: </dt>
                      <dd data-testid="published-date">
                        <strong>
                          {methodology.published ? (
                            <FormattedDate>
                              {methodology.published}
                            </FormattedDate>
                          ) : (
                            'Not yet published'
                          )}
                        </strong>
                      </dd>
                    </div>
                  </dl>
                  {!isEditing && (
                    <>
                      <PrintThisPage
                        analytics={{
                          category: 'Page print',
                          action: 'Print this page link selected',
                        }}
                      />
                      <PageSearchForm inputLabel="Search in this methodology page." />
                    </>
                  )}
                </div>
              </div>
              <MethodologyAccordion
                methodology={methodology}
                sectionKey="content"
                title="Content"
              />
              {!isEditing && methodology.annexes.length ? (
                <h2>Annexes</h2>
              ) : null}
              <MethodologyAccordion
                methodology={methodology}
                sectionKey="annexes"
                title="Annexes"
              />
            </section>
          </div>
        </>
      )}
    </EditingContextProvider>
  );
};

const MethodologyContentPage = ({
  match,
}: RouteComponentProps<MethodologyRouteParams>) => {
  const { methodologyId } = match.params;

  const { value, isLoading } = useAsyncHandledRetry<
    MethodologyContextState
  >(async () => {
    const methodology = await methodologyService.getMethodologyContent(
      methodologyId,
    );
    const canUpdateMethodology = await permissionService.canUpdateMethodology(
      methodologyId,
    );

    return {
      methodology,
      canUpdateMethodology,
    };
  }, [methodologyId]);

  return (
    <LoadingSpinner loading={isLoading}>
      {value ? (
        <MethodologyProvider value={value}>
          <MethodologyContentPageInternal />
        </MethodologyProvider>
      ) : (
        <WarningMessage>Could not load methodology</WarningMessage>
      )}
    </LoadingSpinner>
  );
};

export default MethodologyContentPage;
