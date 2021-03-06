import preReleaseUserService from '@admin/services/preReleaseUserService';
import Button from '@common/components/Button';
import ButtonGroup from '@common/components/ButtonGroup';
import ButtonText from '@common/components/ButtonText';
import Form from '@common/components/form/Form';
import FormFieldTextInput from '@common/components/form/FormFieldTextInput';
import Gate from '@common/components/Gate';
import InsetText from '@common/components/InsetText';
import LoadingSpinner from '@common/components/LoadingSpinner';
import WarningMessage from '@common/components/WarningMessage';
import useAsyncRetry from '@common/hooks/useAsyncRetry';
import useFormSubmit from '@common/hooks/useFormSubmit';
import useToggle from '@common/hooks/useToggle';
import { mapFieldErrors } from '@common/validation/serverValidations';
import Yup from '@common/validation/yup';
import { Formik } from 'formik';
import React from 'react';

interface FormValues {
  email: string;
}

const errorMappings = [
  mapFieldErrors<FormValues>({
    target: 'email',
    messages: {
      USER_ALREADY_EXISTS: 'User with this email already exists',
    },
  }),
];

interface Props {
  releaseId: string;
  isReleaseLive?: boolean;
}

const formId = 'preReleaseUserAccessForm';

const PreReleaseUserAccessForm = ({
  releaseId,
  isReleaseLive = false,
}: Props) => {
  const [isRemoving, toggleRemoving] = useToggle(false);

  const {
    value: users = [],
    isLoading,
    error,
    setState: setUsers,
  } = useAsyncRetry(() => preReleaseUserService.getUsers(releaseId), [
    releaseId,
  ]);

  const handleSubmit = useFormSubmit<FormValues>(async (values, actions) => {
    const newUser = await preReleaseUserService.inviteUser(
      releaseId,
      values.email,
    );

    setUsers({
      value: [...users, newUser],
    });

    actions.resetForm();
  }, errorMappings);

  if (error) {
    return <WarningMessage>Could not load pre-release users</WarningMessage>;
  }

  return (
    <LoadingSpinner loading={isLoading}>
      <Gate
        condition={!isReleaseLive}
        fallback={
          <WarningMessage>
            This release has been published and can no longer be updated.
          </WarningMessage>
        }
      >
        <Formik<FormValues>
          enableReinitialize
          initialValues={{
            email: '',
          }}
          validationSchema={Yup.object<FormValues>({
            email: Yup.string()
              .required('Enter an email address')
              .email('Enter a valid @education.gov.uk email address')
              .test({
                name: 'email format',
                message: 'Enter a valid @education.gov.uk email address',
                test: (value: string) => {
                  if (value) {
                    if (
                      value ===
                      'simulate-delivered@notifications.service.gov.uk'
                    ) {
                      return true;
                    }
                    const emailSegments = value.split('@');
                    return (
                      emailSegments.length === 2 &&
                      emailSegments[1] === 'education.gov.uk'
                    );
                  }
                  return false;
                },
              }),
          })}
          onSubmit={handleSubmit}
        >
          {form => (
            <Form id={formId}>
              <FormFieldTextInput<FormValues>
                label="Invite new user by email"
                name="email"
                className="govuk-!-width-one-third"
              />

              <ButtonGroup>
                <Button
                  type="submit"
                  disabled={form.isSubmitting || isRemoving}
                >
                  {form.isSubmitting ? 'Inviting new user' : 'Invite new user'}
                </Button>
              </ButtonGroup>
            </Form>
          )}
        </Formik>
      </Gate>

      {users.length > 0 ? (
        <>
          <InsetText>
            <p>
              These people will have access to a preview of the release 24 hours
              before the scheduled publish date.
            </p>
          </InsetText>

          <table>
            <thead>
              <tr>
                <th>User email</th>
                {!isReleaseLive && <th />}
              </tr>
            </thead>
            <tbody>
              {users.map(user => (
                <tr key={user.email}>
                  <td>{user.email}</td>

                  {!isReleaseLive && (
                    <td className="dfe-align--right">
                      <ButtonText
                        disabled={isRemoving}
                        onClick={async () => {
                          toggleRemoving.on();

                          await preReleaseUserService.removeUser(
                            releaseId,
                            user.email,
                          );

                          setUsers({
                            value: users.filter(u => u.email !== user.email),
                          });
                          toggleRemoving.off();
                        }}
                      >
                        Remove
                      </ButtonText>
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </>
      ) : (
        <InsetText>No pre-release users have been invited.</InsetText>
      )}
    </LoadingSpinner>
  );
};

export default PreReleaseUserAccessForm;
