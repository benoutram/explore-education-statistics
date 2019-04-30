import createErrorHelper from '@common/lib/validation/createErrorHelper';
import { Omit } from '@common/types/util';
import { FieldArray } from 'formik';
import get from 'lodash/get';
import React from 'react';
import FormCheckboxGroup, { FormCheckboxGroupProps } from './FormCheckboxGroup';
import { onAllChange, onChange } from './util/checkboxGroupFieldHelpers';

type Props<FormValues> = {
  name: keyof FormValues | string;
  showError?: boolean;
} & Omit<FormCheckboxGroupProps, 'value'>;

const FormFieldCheckboxGroup = <T extends {}>(props: Props<T>) => {
  const { error, name, options, showError = true } = props;

  return (
    <FieldArray name={name}>
      {fieldArrayProps => {
        const { form } = fieldArrayProps;
        const { getError } = createErrorHelper(form);

        let errorMessage = error || getError(name);

        if (!showError) {
          errorMessage = '';
        }

        return (
          <FormCheckboxGroup
            {...props}
            error={errorMessage}
            options={options}
            value={get(form.values, name)}
            onAllChange={onAllChange(fieldArrayProps, options)}
            onChange={onChange(fieldArrayProps)}
          />
        );
      }}
    </FieldArray>
  );
};

export default FormFieldCheckboxGroup;
