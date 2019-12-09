import ErrorSummary, {
  ErrorSummaryMessage,
} from '@common/components/ErrorSummary';
import createErrorHelper from '@common/lib/validation/createErrorHelper';
import { connect, FormikContext } from 'formik';
import camelCase from 'lodash/camelCase';
import get from 'lodash/get';
import React, { ReactNode, useEffect, useState } from 'react';

interface Props {
  children: ReactNode;
  id: string;
  submitId?: string;
  displayErrorMessageOnUncaughtErrors?: boolean;
}

/**
 * Form wrapper to integrate with Formik.
 *
 * This provides a bunch of conveniences for displaying errors
 * and linking to them correctly (in error message hrefs) as
 * long as certain conventions are followed.
 *
 * Fields with errors should have ids which share the form's
 * id and camelCased value key in Formik
 * e.g. if form id is `timePeriodForm`, then a Formik value with a
 * key of `startDate` should have a field with an id of
 * `timePeriodForm-startDate`.
 *
 * Additional errors upon submitting the form e.g. from server
 * requests will also be added to the error summary. These
 * will link to the `submitId` prop.
 */
const Form = ({
  children,
  id,
  submitId = `${id}-submit`,
  formik,
  displayErrorMessageOnUncaughtErrors = false,
}: Props & { formik: FormikContext<{}> }) => {
  const { errors, touched } = formik;

  const { getAllErrors } = createErrorHelper({
    errors,
    touched,
  });

  const [submitted, setSubmitted] = useState(false);
  const [submitError, setSubmitError] = useState<ErrorSummaryMessage>();

  useEffect(() => {
    if (!formik.submitCount) {
      setSubmitError(undefined);
    }
  }, [submitError, formik.submitCount]);

  const summaryErrors: ErrorSummaryMessage[] = Object.entries(getAllErrors())
    .filter(([errorName]) => get(touched, errorName))
    .map(([errorName, message]) => ({
      id: `${id}-${camelCase(errorName)}`,
      message: typeof message === 'string' ? message : '',
    }));

  const allErrors = submitError
    ? [...summaryErrors, submitError]
    : summaryErrors;

  return (
    <form
      id={id}
      onSubmit={async event => {
        setSubmitted(true);
        setSubmitError(undefined);
        event.preventDefault();

        try {
          await formik.submitForm();
        } catch (error) {
          if (error) {
            if (displayErrorMessageOnUncaughtErrors) {
              setSubmitError({
                id: submitId,
                message: error.message,
              });
            } else {
              throw error;
            }
          }
        }
      }}
    >
      <ErrorSummary
        errors={allErrors}
        id={`${id}-summary`}
        focusOnError={submitted}
      />

      {children}
    </form>
  );
};

export default connect<Props, {}>(Form);
