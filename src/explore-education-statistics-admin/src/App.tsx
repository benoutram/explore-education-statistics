import ProtectedRoute from '@admin/components/ProtectedRoute';
import CreatePublicationPage from '@admin/pages/create-publication/CreatePublicationPage';
import CreateReleasePage from "@admin/pages/release/create-release/CreateReleasePage";
import MockSignInProcess from '@admin/pages/sign-in/mock/MockSignInProcess';
import MockSignOutProcess from '@admin/pages/sign-in/mock/MockSignOutProcess';
import SignedOutPage from '@admin/pages/sign-in/SignedOutPage';
import SignInPage from '@admin/pages/sign-in/SignInPage';
import signInRoutes from '@admin/routes/sign-in/routes';
import dashboardRoutes from '@admin/routes/dashboard/routes';
import releaseRoutes from '@admin/routes/edit-release/routes';
import publicationRoutes from '@admin/routes/edit-publication/routes';
import PrototypeLoginService from '@admin/services/PrototypeLoginService';
import React from 'react';
import { Route } from 'react-router';
import { BrowserRouter } from 'react-router-dom';

import './App.scss';

import { LoginContext } from './components/Login';
import AdminDashboardPage from './pages/admin-dashboard/AdminDashboardPage';
import IndexPage from './pages/IndexPage';
import PrototypeAdminDashboard from './pages/prototypes/PrototypeAdminDashboard';

import PrototypeChartTest from './pages/prototypes/PrototypeChartTest';
import AdminDocumentationGlossary from './pages/prototypes/PrototypeDocumentationGlossary';
import AdminDocumentationHome from './pages/prototypes/PrototypeDocumentationHome';
import PublicationAssignMethodology from './pages/prototypes/PrototypePublicationPageAssignMethodology';
import PublicationConfirmNew from './pages/prototypes/PrototypePublicationPageConfirmNew';

import PublicationCreateNew from './pages/prototypes/PrototypePublicationPageCreateNew';

import PublicationEditPage from './pages/prototypes/PrototypePublicationPageEditAbsence';
import PublicationEditUnresolvedComments from './pages/prototypes/PrototypePublicationPageEditAbsenceUnresolvedComments';
import PublicationEditNew from './pages/prototypes/PrototypePublicationPageEditNew';
import PublicationCreateNewAbsence from './pages/prototypes/PrototypePublicationPageNewAbsence';
import PublicationCreateNewAbsenceConfig from './pages/prototypes/PrototypePublicationPageNewAbsenceConfig';
import PublicationCreateNewAbsenceConfigEdit from './pages/prototypes/PrototypePublicationPageNewAbsenceConfigEdit';
import PublicationCreateNewAbsenceData from './pages/prototypes/PrototypePublicationPageNewAbsenceData';
import PublicationCreateNewAbsenceSchedule from './pages/prototypes/PrototypePublicationPageNewAbsenceSchedule';
import PublicationCreateNewAbsenceScheduleEdit from './pages/prototypes/PrototypePublicationPageNewAbsenceScheduleEdit';
import PublicationCreateNewAbsenceStatus from './pages/prototypes/PrototypePublicationPageNewAbsenceStatus';
import PublicationCreateNewAbsenceTable from './pages/prototypes/PrototypePublicationPageNewAbsenceTable';
import PublicationCreateNewAbsenceViewTables from './pages/prototypes/PrototypePublicationPageNewAbsenceViewTables';
import PublicationReviewPage from './pages/prototypes/PrototypePublicationPageReviewAbsence';
import ReleaseCreateNew from './pages/prototypes/PrototypeReleasePageCreateNew';
import PrototypesIndexPage from './pages/prototypes/PrototypesIndexPage';

function App() {
  return (
    <BrowserRouter>
      {/* Non-Prototype Routes*/}
      <ProtectedRoute
        exact
        path={dashboardRoutes.adminDashboard}
        component={AdminDashboardPage}
      />

      <ProtectedRoute
        exact
        path={signInRoutes.signIn}
        component={SignInPage}
        redirectIfNotLoggedIn={false}
      />

      <ProtectedRoute
        exact
        path={signInRoutes.signOut}
        component={SignedOutPage}
        redirectIfNotLoggedIn={false}
      />

      <ProtectedRoute
        exact
        path={publicationRoutes.createPublication.route}
        component={CreatePublicationPage}
      />

      <ProtectedRoute
        exact
        path={releaseRoutes.createReleaseRoute.route}
        component={CreateReleasePage}
      />

      {releaseRoutes.manageReleaseRoutes.map(route => (
        <ProtectedRoute
          exact
          key={route.path}
          path={route.path}
          component={route.component}
        />
      ))}

      {process.env.USE_MOCK_API === 'true' && (
        <>
          <Route
            exact
            path={signInRoutes.signInViaApiLink}
            component={MockSignInProcess}
          />
          <Route
            exact
            path={signInRoutes.signOutViaApiLink}
            component={MockSignOutProcess}
          />
        </>
      )}

      {/* Prototype Routes */}
      <Route exact path="/index" component={IndexPage} />

      <LoginContext.Provider value={PrototypeLoginService.login()}>
        <Route exact path="/prototypes/" component={PrototypesIndexPage} />

        <Route
          exact
          path="/prototypes/admin-dashboard"
          component={PrototypeAdminDashboard}
        />

        <Route exact path="/prototypes/charts" component={PrototypeChartTest} />

        <Route
          exact
          path="/prototypes/publication-edit"
          component={PublicationEditPage}
        />
        <Route
          exact
          path="/prototypes/publication-unresolved-comments"
          component={PublicationEditUnresolvedComments}
        />
        <Route
          exact
          path="/prototypes/publication-review"
          component={PublicationReviewPage}
        />
        <Route
          exact
          path="/prototypes/publication-create-new"
          component={PublicationCreateNew}
        />
        <Route
          exact
          path="/prototypes/publication-assign-methodology"
          component={PublicationAssignMethodology}
        />
        <Route
          exact
          path="/prototypes/publication-confirm-new"
          component={PublicationConfirmNew}
        />
        <Route
          exact
          path="/prototypes/publication-edit-new"
          component={PublicationEditNew}
        />
        <Route
          exact
          path="/prototypes/release-create-new"
          component={ReleaseCreateNew}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence"
          component={PublicationCreateNewAbsence}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence-config"
          component={PublicationCreateNewAbsenceConfig}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence-config-edit"
          component={PublicationCreateNewAbsenceConfigEdit}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence-data"
          component={PublicationCreateNewAbsenceData}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence-table"
          component={PublicationCreateNewAbsenceTable}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence-view-table"
          component={PublicationCreateNewAbsenceViewTables}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence-schedule"
          component={PublicationCreateNewAbsenceSchedule}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence-schedule-edit"
          component={PublicationCreateNewAbsenceScheduleEdit}
        />
        <Route
          exact
          path="/prototypes/publication-create-new-absence-status"
          component={PublicationCreateNewAbsenceStatus}
        />
        <Route
          exact
          path="/prototypes/documentation/"
          component={AdminDocumentationHome}
        />
        <Route
          exact
          path="/prototypes/documentation/glossary"
          component={AdminDocumentationGlossary}
        />
        <Route
          exact
          path="/prototypes/documentation/style-guide"
          component={AdminDocumentationGlossary}
        />
      </LoginContext.Provider>
    </BrowserRouter>
  );
}

export default App;
