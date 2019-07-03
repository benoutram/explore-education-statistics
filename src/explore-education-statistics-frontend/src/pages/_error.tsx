import { NextContext } from 'next';
import React, { Component } from 'react';
import Page from '../components/Page';
import PageTitle from '../components/PageTitle';

interface Props {
  errorMessage: string;
  statusCode: number;
}

class ErrorPage extends Component<Props> {
  private static statusCodeTitles: { [code: number]: string } = {
    404: 'Page not found',
    500: "Sorry, there's a problem with the service",
  };

  public static getInitialProps({ res, err }: NextContext): Props {
    const statusCode = res ? res.statusCode : 500;
    const errorMessage = err ? err.message : '';

    return {
      errorMessage,
      statusCode,
    };
  }

  private getStatusCodePage() {
    const { statusCode } = this.props;

    switch (statusCode) {
      case 404:
        return (
          <>
            <PageTitle title={ErrorPage.statusCodeTitles[404]} />

            <p>If you typed the web address, check it's correct.</p>
            <p>
              If you cut and pasted the web address, check you copied the entire
              address.
            </p>
            <p>
              If the web address is correct or you clicked a link or button and
              ended up on this page,{' '}
              <a className="govuk-link" href="/contact">
                contact our Explore education statistics team
              </a>{' '}
              if you need any help or support.
            </p>
          </>
        );
      case 500:
        return (
          <>
            <PageTitle title={ErrorPage.statusCodeTitles[500]} />

            <p>Try again later.</p>
            <p>
              In the meantime, if you need any help or support{' '}
              <a className="govuk-link" href="/contact">
                contact our Explore education statistics team
              </a>
              .
            </p>
          </>
        );
      default:
        return (
          <>
            <PageTitle title={ErrorPage.statusCodeTitles[500]} />

            <p>Try again later.</p>
            <p>
              In the meantime, if you need any help or support{' '}
              <a className="govuk-link" href="/contact">
                contact our Explore education statistics team
              </a>
              .
            </p>
          </>
        );
    }
  }

  public render() {
    return <Page>{this.getStatusCodePage()}</Page>;
  }
}

export default ErrorPage;
