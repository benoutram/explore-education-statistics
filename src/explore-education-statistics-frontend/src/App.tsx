import React, { Component } from 'react';
import {
  BrowserRouter as Router,
  HashRouter,
  Route,
  Switch,
} from 'react-router-dom';
import Breadcrumbs from './components/Breadcrumbs';
import CookiesPage from './pages/CookiesPage';
import FeedbackPage from './pages/FeedbackPage';
import HomePage from './pages/HomePage';
import NotFoundPage from './pages/NotFoundPage';
import PrivacyPage from './pages/PrivacyPage';
import PublicationPage from './pages/PublicationPage';
import PublicationsPage from './pages/PublicationsPage';
import ThemePage from './pages/ThemePage';
import ThemesPage from './pages/ThemesPage';
import TopicPage from './pages/TopicPage';
import TopicsPage from './pages/TopicsPage';

import './App.scss';

class App extends Component {
  public render() {
    return (
      <div className="App">
        <Router>
          <div>
            <div className="govuk-phase-banner">
              <p className="govuk-phase-banner__content">
                <strong className="govuk-tag govuk-phase-banner__content__tag ">
                  alpha
                </strong>
                <span className="govuk-phase-banner__text">
                  This is a new service – your
                  <a className="govuk-link" href="/feedback">
                    feedback
                  </a>{' '}
                  will help us to improve it.
                </span>
              </p>
            </div>
            <Breadcrumbs current={'current page'} />
            <main
              className="govuk-main-wrapper app-main-class"
              id="main-content"
              role="main"
            >
              <Switch>
                <Route exact path="/" component={HomePage} />
                <Route exact path="/cookies" component={CookiesPage} />
                <Route exact path="/privacy-policy" component={PrivacyPage} />
                <Route exact path="/feedback" component={FeedbackPage} />

                <Route exact path="/themes" component={ThemesPage} />
                <Route exact path="/themes/:theme" component={ThemePage} />
                <Route
                  exact
                  path="/themes/:theme/:topic"
                  component={TopicPage}
                />
                <Route
                  exact
                  path="/themes/:theme/:topic/:publication"
                  component={PublicationPage}
                />

                <Route exact path="/topics" component={TopicsPage} />
                <Route exact path="/topics/:topic" component={TopicPage} />
                <Route
                  exact
                  path="/topics/:topic/:publication"
                  component={PublicationPage}
                />

                <Route
                  exact
                  path="/publications/"
                  component={PublicationsPage}
                />
                <Route
                  exact
                  path="/publications/:publication"
                  component={PublicationPage}
                />

                <Route component={NotFoundPage} />
              </Switch>
            </main>
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
