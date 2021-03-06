import ButtonText from '@common/components/ButtonText';
import useMounted from '@common/hooks/useMounted';
import { OmitStrict, PartialBy } from '@common/types/util';
import naturalOrderBy, {
  OrderDirection,
  OrderKeys,
} from '@common/utils/array/naturalOrderBy';
import classNames from 'classnames';
import React, {
  FocusEventHandler,
  memo,
  MouseEvent,
  MouseEventHandler,
  useCallback,
  useMemo,
  useRef,
} from 'react';
import FormCheckbox, {
  CheckboxChangeEventHandler,
  FormCheckboxProps,
} from './FormCheckbox';
import styles from './FormCheckboxGroup.module.scss';
import FormFieldset, { FormFieldsetProps } from './FormFieldset';

export type CheckboxOption = PartialBy<
  OmitStrict<FormCheckboxProps, 'name' | 'checked' | 'onChange'>,
  'id'
>;

export type CheckboxGroupAllChangeEvent = MouseEvent<HTMLButtonElement>;

export type CheckboxGroupAllChangeEventHandler = (
  event: CheckboxGroupAllChangeEvent,
  checked: boolean,
) => void;

interface BaseFormCheckboxGroupProps {
  disabled?: boolean;
  id: string;
  name: string;
  options: CheckboxOption[];
  selectAll?: boolean;
  selectAllText?: (isAllChecked: boolean, options: CheckboxOption[]) => string;
  small?: boolean;
  order?: OrderKeys<CheckboxOption>;
  orderDirection?: OrderDirection | OrderDirection[];
  value: string[];
  onAllChange?: CheckboxGroupAllChangeEventHandler;
  onBlur?: FocusEventHandler<HTMLInputElement>;
  onChange?: CheckboxChangeEventHandler;
}

const getDefaultSelectAllText = (
  isAllChecked: boolean,
  opts: CheckboxOption[],
) => `${isAllChecked ? 'Unselect' : 'Select'} all ${opts.length} options`;

/**
 * Basic checkbox group that should be used as a controlled component.
 */
export const BaseFormCheckboxGroup = ({
  disabled,
  value = [],
  id,
  name,
  options,
  selectAll = false,
  selectAllText = getDefaultSelectAllText,
  small,
  order = ['label'],
  orderDirection = ['asc'],
  onBlur,
  onChange,
  onAllChange,
}: BaseFormCheckboxGroupProps) => {
  const ref = useRef<HTMLDivElement>(null);

  useMounted(() => {
    if (ref.current) {
      import('govuk-frontend/govuk/components/checkboxes/checkboxes').then(
        ({ default: GovUkCheckboxes }) => {
          if (ref.current) {
            new GovUkCheckboxes(ref.current).init();
          }
        },
      );
    }
  });

  const isAllChecked = useMemo(() => {
    return options.every(option => value.indexOf(option.value) > -1);
  }, [options, value]);

  const handleAllChange: MouseEventHandler<HTMLButtonElement> = useCallback(
    event => {
      if (onAllChange) {
        onAllChange(event, isAllChecked);
      }
    },
    [isAllChecked, onAllChange],
  );

  return (
    <div
      className={classNames('govuk-checkboxes', {
        'govuk-checkboxes--small': small,
      })}
      ref={ref}
    >
      {options.length > 1 && selectAll && (
        <ButtonText
          id={`${id}-all`}
          onClick={handleAllChange}
          className={styles.selectAll}
          underline={false}
        >
          {selectAllText(isAllChecked, options)}
        </ButtonText>
      )}

      {naturalOrderBy(options, order, orderDirection).map(option => (
        <FormCheckbox
          disabled={disabled}
          {...option}
          id={
            option.id
              ? `${id}-${option.id}`
              : `${id}-${option.value.replace(/\s/g, '-')}`
          }
          name={name}
          key={option.value}
          checked={value.indexOf(option.value) > -1}
          onBlur={onBlur}
          onChange={onChange}
        />
      ))}

      {options.length === 0 && <p>No options available.</p>}
    </div>
  );
};

export type FormCheckboxGroupProps = BaseFormCheckboxGroupProps &
  OmitStrict<FormFieldsetProps, 'useFormId' | 'onBlur' | 'onFocus'> & {
    onFieldsetBlur?: FocusEventHandler<HTMLFieldSetElement>;
    onFieldsetFocus?: FocusEventHandler<HTMLFieldSetElement>;
  };

const FormCheckboxGroup = ({
  onFieldsetBlur,
  onFieldsetFocus,
  ...props
}: FormCheckboxGroupProps) => {
  return (
    <FormFieldset
      {...props}
      useFormId={false}
      onBlur={onFieldsetBlur}
      onFocus={onFieldsetFocus}
    >
      <BaseFormCheckboxGroup {...props} />
    </FormFieldset>
  );
};

export default memo(FormCheckboxGroup);
