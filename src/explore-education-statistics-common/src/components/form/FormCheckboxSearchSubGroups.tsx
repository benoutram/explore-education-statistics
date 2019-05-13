import FormCheckboxGroup from '@common/components/form/FormCheckboxGroup';
import FormFieldset, {
  FormFieldsetProps,
} from '@common/components/form/FormFieldset';
import useMounted from '@common/hooks/useMounted';
import camelCase from 'lodash/camelCase';
import sum from 'lodash/sum';
import React, { useState } from 'react';
import styles from './FormCheckboxSearchGroup.module.scss';
import FormCheckboxSubGroups, {
  FormCheckboxSubGroupsProps,
} from './FormCheckboxSubGroups';
import FormTextSearchInput from './FormTextSearchInput';

export interface FormCheckboxSearchSubGroupsProps
  extends FormCheckboxSubGroupsProps {
  hideCount?: boolean;
  searchLabel?: string;
}

const FormCheckboxSearchSubGroups = ({
  hideCount = false,
  searchLabel = 'Search options',
  legend,
  legendHidden,
  legendSize,
  hint,
  error,
  ...props
}: FormCheckboxSearchSubGroupsProps) => {
  const { id, name, options, onAllChange, value = [] } = props;
  const fieldsetProps: FormFieldsetProps = {
    id,
    legend,
    legendHidden,
    legendSize,
    hint,
    error,
  };

  const [searchTerm, setSearchTerm] = useState('');
  const { isMounted } = useMounted();

  let filteredOptions = options;

  if (searchTerm) {
    filteredOptions = options
      .filter(optionGroup =>
        optionGroup.options.some(
          option =>
            value.indexOf(option.value) > -1 ||
            new RegExp(searchTerm, 'i').test(option.label),
        ),
      )
      .map(optionGroup => ({
        ...optionGroup,
        options: optionGroup.options.filter(
          option =>
            value.indexOf(option.value) > -1 ||
            new RegExp(searchTerm, 'i').test(option.label),
        ),
      }));
  }

  const selectedCount = sum(
    options.flatMap(optionGroup =>
      optionGroup.options.reduce(
        (acc, option) => (value.indexOf(option.value) > -1 ? acc + 1 : acc),
        0,
      ),
    ),
  );

  return (
    <>
      {isMounted ? (
        <FormFieldset {...fieldsetProps}>
          <div className={styles.inputContainer}>
            <FormTextSearchInput
              id={`${id}-search`}
              name={`${name}-search`}
              onChange={event => setSearchTerm(event.target.value)}
              label={searchLabel}
              width={20}
            />

            {selectedCount > 0 && !hideCount && (
              <div className="govuk-!-margin-top-2">
                <span className="govuk-tag">{`${selectedCount} selected`}</span>
              </div>
            )}
          </div>

          <div className={styles.optionsContainer}>
            {filteredOptions.map(optionGroup => (
              <FormCheckboxGroup
                {...props}
                id={
                  optionGroup.id
                    ? optionGroup.id
                    : `${id}-${camelCase(optionGroup.legend)}`
                }
                key={optionGroup.legend}
                legend={optionGroup.legend}
                legendSize="s"
                options={optionGroup.options}
                onAllChange={event => {
                  if (onAllChange) {
                    onAllChange(event, optionGroup.options);
                  }
                }}
              />
            ))}
          </div>
        </FormFieldset>
      ) : (
        <FormCheckboxSubGroups {...fieldsetProps} {...props} />
      )}
    </>
  );
};

export default FormCheckboxSearchSubGroups;
