import Button from '@common/components/Button';
import { FormGroup } from '@common/components/form';
import { FormikProps } from 'formik';
import React, { MouseEventHandler } from 'react';
import { InjectedWizardProps } from './Wizard';

interface Props {
  form: FormikProps<{}>;
  formId: string;
  goToPreviousStep: InjectedWizardProps['goToPreviousStep'];
  onPreviousStep?: MouseEventHandler;
  stepNumber: InjectedWizardProps['stepNumber'];
  submitText?: string;
  submittingText?: string;
}

const WizardStepFormActions = ({
  form,
  formId,
  goToPreviousStep,
  onPreviousStep,
  stepNumber,
  submitText = 'Next step',
  submittingText = 'Submitting',
}: Props) => {
  return (
    <FormGroup>
      <Button
        disabled={form.isSubmitting}
        id={`${formId}-submit`}
        type="submit"
      >
        {form.isSubmitting ? submittingText : submitText}
      </Button>

      {stepNumber > 1 && (
        <Button
          type="button"
          variant="secondary"
          onClick={event => {
            if (onPreviousStep) {
              onPreviousStep(event);
            }

            if (!event.isDefaultPrevented()) {
              goToPreviousStep();
            }
          }}
        >
          Previous step
        </Button>
      )}
    </FormGroup>
  );
};

export default WizardStepFormActions;
