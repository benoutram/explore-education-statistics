import React, { Component } from 'react';
import api from '../api';
import DataList from '../components/DataList';
import Title from '../components/Title';

interface State {
  data: any[];
}

class Publications extends Component<{}, State> {
  public state = {
    data: [],
  };

  public componentDidMount() {
    api
      .get('publication')
      .then(({ data }) => this.setState({ data }))
      .catch(error => alert(error));
  }

  public render() {
    const { data } = this.state;

    return (
      <div className="govuk-grid-row">
        <div className="govuk-grid-column-two-thirds">
          <Title label="Publications" />
          <DataList data={data} linkIdentifier={window.location.pathname} />
        </div>
      </div>
    );
  }
}

export default Publications;
