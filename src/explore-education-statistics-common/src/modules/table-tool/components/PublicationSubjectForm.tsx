import { Form, FormFieldRadioGroup, Formik } from '@common/components/form';
import SummaryList from '@common/components/SummaryList';
import SummaryListItem from '@common/components/SummaryListItem';
import Yup from '@common/lib/validation/yup';
import { PublicationSubject } from '@common/modules/table-tool/services/tableBuilderService';
import useResetFormOnPreviousStep from '@common/modules/table-tool/components/hooks/useResetFormOnPreviousStep';
import { FormikProps } from 'formik';
import React, { useEffect, useRef, useState } from 'react';
import { InjectedWizardProps } from './Wizard';
import WizardStepFormActions from './WizardStepFormActions';
import WizardStepHeading from './WizardStepHeading';

interface FormValues {
  subjectId: string;
}

export type PublicationSubjectFormSubmitHandler = (values: {
  subjectId: string;
  subjectName: string;
}) => void;

interface Props {
  onSubmit: PublicationSubjectFormSubmitHandler;
  options: PublicationSubject[];
  initialValues?: { subjectId?: string };
}
const initialiseSubjectName = (
  sid: string,
  options: PublicationSubject[],
): string => (options.find(({ id }) => sid === id) || { label: '' }).label;

const PublicationSubjectForm = (props: Props & InjectedWizardProps) => {
  const {
    isActive,
    onSubmit,
    options,
    goToNextStep,
    currentStep,
    stepNumber,
    initialValues: { subjectId: initialSubjectId = '' } = {},
  } = props;

  const [subjectName, setSubjectName] = useState(() =>
    initialiseSubjectName(initialSubjectId, options),
  );

  const formikRef = useRef<Formik<FormValues>>(null);
  const formId = 'publicationSubjectForm';

  useResetFormOnPreviousStep(formikRef, currentStep, stepNumber, () => {
    setSubjectName('');
  });

  const stepHeading = (
    <WizardStepHeading {...props} fieldsetHeading>
      Choose a subject
    </WizardStepHeading>
  );

  const initialValues = {
    subjectId: initialSubjectId,
  };

  useEffect(() => {
    if (formikRef.current) {
      formikRef.current.setValues({
        subjectId: `${initialSubjectId}`,
      });
    }
    setSubjectName(initialiseSubjectName(initialSubjectId, options));
  }, [options, initialSubjectId]);

  return (
    <Formik<FormValues>
      enableReinitialize
      ref={formikRef}
      initialValues={initialValues}
      validateOnBlur={false}
      validateOnChange={false}
      validationSchema={Yup.object<FormValues>({
        subjectId: Yup.string().required('Choose a subject'),
      })}
      onSubmit={async ({ subjectId }) => {
        await onSubmit({
          subjectId,
          subjectName,
        });
        goToNextStep();
      }}
      render={(form: FormikProps<FormValues>) => {
        return isActive ? (
          <Form {...form} id={formId} showSubmitError>
            <FormFieldRadioGroup<FormValues>
              name="subjectId"
              legend={stepHeading}
              legendSize="l"
              options={options.map(option => ({
                label: option.label,
                value: `${option.id}`,
              }))}
              id={`${formId}-subjectId`}
              disabled={form.isSubmitting}
              onChange={(event, option) => {
                setSubjectName(option.label);
              }}
            />

            <WizardStepFormActions {...props} form={form} formId={formId} />
          </Form>
        ) : (
          <>
            {stepHeading}
            <SummaryList noBorder>
              <SummaryListItem term="Subject">{subjectName}</SummaryListItem>
            </SummaryList>
          </>
        );
      }}
    />
  );
};

export default PublicationSubjectForm;
