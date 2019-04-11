import { Form, Formik, FormikErrors, FormikProps, FormikTouched } from 'formik';
import debounce from 'lodash/debounce';
import React, { ChangeEvent, Component, createRef } from 'react';

import SearchableFilterMenus from './SearchableFilterMenus';
import SchoolType from "../../../services/types/SchoolType";
import {PublicationMeta} from "../../../services/tableBuilderService";
import ErrorSummary, {ErrorSummaryMessage} from "../../../components/ErrorSummary";
import Yup from '../../../lib/validation/yup';
import FormFieldSelect from "../../../components/form/FormFieldSelect";
import FormFieldCheckboxGroup from "../../../components/form/FormFieldCheckboxGroup";
import FormFieldset from "../../../components/form/FormFieldset";
import createErrorHelper from "../../../lib/validation/createErrorHelper";
import FormGroup from "../../../components/form/FormGroup";
import FormTextInput from '../../../components/form/FormTextInput';
import Button from '../../../components/Button';

interface FormValues {
  characteristics: string[];
  indicators: string[];
  endYear: number;
  schoolTypes: SchoolType[];
  startYear: number;
}

export type CharacteristicsFilterFormSubmitHandler = (
  values: FormValues,
) => void;

interface Props {
  publicationMeta: Pick<PublicationMeta, 'characteristics' | 'indicators'>;
  onSubmit: CharacteristicsFilterFormSubmitHandler;
}

interface State {
  isSubmitted: boolean;
  openFilters: {
    [key: string]: boolean;
  };
  searchTerm: string;
  submitError: string;
}

class CharacteristicsFilterForm extends Component<Props, State> {
  public state: State = {
    isSubmitted: false,
    openFilters: {},
    searchTerm: '',
    submitError: '',
  };

  private ref = createRef<HTMLDivElement>();

  private yearOptions = [
    { value: 2011, text: '2011/12' },
    { value: 2012, text: '2012/13' },
    { value: 2013, text: '2013/14' },
    { value: 2014, text: '2014/15' },
    { value: 2015, text: '2015/16' },
    { value: 2016, text: '2016/17' },
  ];

  private schoolTypeOptions = [
    {
      id: 'filter-schoolTypes-total',
      label: 'Total',
      value: SchoolType.Total,
    },
    {
      id: 'filter-schoolTypes-primary',
      label: 'Primary',
      value: SchoolType.State_Funded_Primary,
    },
    {
      id: 'filter-schoolTypes-secondary',
      label: 'Secondary',
      value: SchoolType.State_Funded_Secondary,
    },
    {
      id: 'filter-schoolTypes-special',
      label: 'Special',
      value: SchoolType.Special,
    },
  ];

  private yearValues = this.yearOptions.map(({ value }) => value);

  private setDebouncedFilterSearch = debounce(
    (event: ChangeEvent<HTMLInputElement>) => {
      this.setState({
        searchTerm: event.target.value,
      });
    },
    300,
  );

  private getSummaryErrors(
    errors: FormikErrors<FormValues>,
    touched: FormikTouched<FormValues>,
  ) {
    const summaryErrors: ErrorSummaryMessage[] = Object.entries(errors)
      .filter(([errorName]) => touched[errorName as keyof FormValues])
      .map(([errorName, message]) => ({
        id: `filter-${errorName}`,
        message: typeof message === 'string' ? message : '',
      }));

    if (this.state.submitError) {
      summaryErrors.push({
        id: 'submit-button',
        message: 'Could not submit filters. Please try again later.',
      });
    }

    return summaryErrors;
  }

  public render() {
    return (
      <Formik
        initialValues={{
          characteristics: [],
          endYear: 2016,
          indicators: [],
          schoolTypes: [],
          startYear: 2012,
        }}
        validationSchema={Yup.object({
          characteristics: Yup.array().required('Select at least one option'),
          endYear: Yup.number()
            .required('End year is required')
            .oneOf(this.yearValues, 'Must be one of provided years')
            .moreThanOrEqual(
              Yup.ref('startYear'),
              'Must be after or same as start year',
            ),
          indicators: Yup.array().required('Select at least one option'),
          schoolTypes: Yup.array().required('Select at least one option'),
          startYear: Yup.number()
            .required('Start year is required')
            .oneOf(this.yearValues, 'Must be one of provided years')
            .lessThanOrEqual(
              Yup.ref('endYear'),
              'Must be before or same as end year',
            ),
        })}
        onSubmit={async (form, actions) => {
          try {
            await this.props.onSubmit(form);
          } catch (error) {
            this.setState({
              submitError: error.message,
            });
          }

          actions.setSubmitting(false);
        }}
        render={({
          errors,
          touched,
          values,
          ...form
        }: FormikProps<FormValues>) => {
          const { getError } = createErrorHelper({ errors, touched });

          return (
            <div ref={this.ref}>
              <ErrorSummary
                errors={this.getSummaryErrors(errors, touched)}
                id="filter-errors"
              />

              <Form>
                <div className="govuk-grid-row">
                  <div className="govuk-grid-column-one-half govuk-form-group">
                    <FormFieldset
                      id="years"
                      legend="Academic years"
                      hint="Filter statistics by a given start and end date"
                    >
                      <FormFieldSelect<FormValues>
                        id="filter-startYear"
                        label="Start year"
                        name="startYear"
                        options={this.yearOptions}
                      />
                      <FormFieldSelect<FormValues>
                        id="filter-endYear"
                        label="End year"
                        name="endYear"
                        options={this.yearOptions}
                      />
                    </FormFieldset>
                  </div>
                  <div className="govuk-grid-column-one-half govuk-form-group">
                    <FormFieldCheckboxGroup<FormValues>
                      id="filter-schoolTypes"
                      name="schoolTypes"
                      legend="School types"
                      hint="Filter statistics by number of pupils in school type(s)"
                      options={this.schoolTypeOptions}
                      selectAll
                    />
                  </div>
                  <div className="govuk-grid-column-one-half govuk-form-group">
                    <FormFieldset
                      id="filter-indicators"
                      legend="Indicators"
                      hint="Filter by at least one statistical indicator from the publication"
                      error={getError('indicators')}
                    >
                      <SearchableFilterMenus<FormValues>
                        menuOptions={this.props.publicationMeta.indicators}
                        name="indicators"
                        searchTerm={this.state.searchTerm}
                        values={values.indicators}
                      />
                    </FormFieldset>
                  </div>
                  <div className="govuk-grid-column-one-half govuk-form-group">
                    <FormFieldset
                      id="filter-characteristics"
                      legend="Characteristics"
                      hint="Filter by at least one pupil characteristic from the publication"
                      error={getError('characteristics')}
                    >
                      <SearchableFilterMenus<FormValues>
                        menuOptions={this.props.publicationMeta.characteristics}
                        name="characteristics"
                        searchTerm={this.state.searchTerm}
                        values={values.characteristics}
                      />
                    </FormFieldset>
                  </div>
                </div>

                <FormGroup>
                  <h3>Can't find what you're looking for?</h3>

                  <FormTextInput
                    id="characteristic-search"
                    label="Search for an indicator or a characteristic"
                    name="characteristicSearch"
                    onChange={event => {
                      event.persist();
                      this.setDebouncedFilterSearch(event);
                    }}
                    width={20}
                  />
                </FormGroup>

                <Button
                  disabled={form.isSubmitting}
                  id="submit-button"
                  onClick={event => {
                    event.preventDefault();

                    // Manually validate/submit so we can scroll
                    // back to the top of the form if there are errors
                    form.validateForm().then(validationErrors => {
                      form.submitForm();

                      if (
                        Object.keys(validationErrors).length > 0 &&
                        this.ref.current
                      ) {
                        this.ref.current.scrollIntoView({
                          behavior: 'smooth',
                          block: 'start',
                        });
                      }
                    });
                  }}
                  type="submit"
                >
                  {form.isSubmitting && form.isValid
                    ? 'Submitting'
                    : 'Confirm filters'}
                </Button>
              </Form>
            </div>
          );
        }}
      />
    );
  }
}

export default CharacteristicsFilterForm;
