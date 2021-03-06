import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { LoginService } from './services/login/login.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { AuthGuard } from './services/auth.guard';
import { RoleGuardService } from './services/role.guard.service';
import { HttpClientModule } from '@angular/common/http';
import { AdminNavComponent } from './components/admin-nav/admin-nav.component';
import { UsersListComponent, UserTypePipe } from './components/users/users.component';
import { UsersService } from './services/users.service';
import { HttpModule } from '@angular/http';
import { SportsComponent, SportPlayersPipe } from './components/sports/sports.component';
import { TeamsComponent } from './components/teams/teams.component';
import { EncountersComponent, ListToSingleString } from './components/encounters/encounters.component';
import { AddUserComponent } from './components/add-user/add-user.component';
import { ModifyUserComponent } from './components/modify-user/modify-user.component';
import { AddTeamComponent } from './components/add-team/add-team.component';
import { FollowerNavComponent } from './components/follower-nav/follower-nav.component';
import { FavoritesComponent } from './components/favorites/favorites.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { CompareValidatorDirective } from './shared/compare-validator.directive';
import { MaterialModule } from './material/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AddSportComponent } from './components/add-sport/add-sport.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { YesNoDialogComponent } from './components/yes-no-dialog/yes-no-dialog.component';
import { ModifyTeamComponent } from './components/modify-team/modify-team.component';
import { LogsComponent } from './components/logs/logs.component';
import { AddEncounterComponent } from './components/add-encounter/add-encounter.component';
import { AddEncountersResultComponent } from './components/add-encounters-result/add-encounters-result.component';
import { GenerateFixtureComponent } from './components/generate-fixture/generate-fixture.component';
import { FollowTeamsComponent } from './components/follow-teams/follow-teams.component';
import { CommentsComponent, EncounterLegibleNamePipe, EncounterLegibleNameAsync } from './components/comments/comments.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { CommonModule } from '@angular/common';
import { FlatpickrModule } from 'angularx-flatpickr';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { TeamPositionComponent } from './components/team-position/team-position.component';

export function tokenGetter() {
  return localStorage.getItem('access_token');
}

const appRoutes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'users',
    component: UsersListComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: 'Administrator'
    }
  },
  {
    path: 'sports',
    component: SportsComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: 'Administrator'
    }
  },
  {
    path: 'teams',
    component: TeamsComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: 'Administrator'
    }
  },
  {
    path: 'encounters',
    component: EncountersComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: 'Administrator'
    }
  },
  {
    path: 'logs',
    component: LogsComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: 'Administrator'
    }
  },
  {
    path: 'calendar',
    component: CalendarComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: ['Follower', 'Administrator']
    }
  },
  {
    path: 'favorites',
    component: FavoritesComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: ['Follower', 'Administrator']
    }
  },
  {
    path: 'positions',
    component: TeamPositionComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: ['Follower', 'Administrator']
    }
  },
  {
    path: 'followTeams',
    component: FollowTeamsComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: ['Follower', 'Administrator']
    }
  },
  {
    path: 'comments',
    component: CommentsComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: ['Follower', 'Administrator']
    }
  },
  {
    path: '404',
    component: NotFoundComponent
  },
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/404'
  }
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AdminNavComponent,
    UsersListComponent,
    SportsComponent,
    TeamsComponent,
    AddUserComponent,
    EncountersComponent,
    ModifyUserComponent,
    AddTeamComponent,
    FollowerNavComponent,
    FavoritesComponent,
    CalendarComponent,
    CompareValidatorDirective,
    AddSportComponent,
    NotFoundComponent,
    YesNoDialogComponent,
    ModifyTeamComponent,
    UserTypePipe,
    EncounterLegibleNamePipe,
    EncounterLegibleNameAsync,
    SportPlayersPipe,
    ListToSingleString,
    LogsComponent,
    AddEncounterComponent,
    AddEncountersResultComponent,
    GenerateFixtureComponent,
    FollowTeamsComponent,
    CommentsComponent,
    TeamPositionComponent
  ],
  entryComponents: [
    AddUserComponent,
    AddSportComponent,
    AddTeamComponent,
    ModifyUserComponent,
    ModifyTeamComponent,
    YesNoDialogComponent,
    AddEncounterComponent,
    AddEncountersResultComponent,
    GenerateFixtureComponent
  ],
  imports: [
    CommonModule,
    NgbModalModule,
    FlatpickrModule.forRoot(),
    BrowserAnimationsModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    MaterialModule,
    ReactiveFormsModule,
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true }
    ),
    HttpClientModule,
    HttpModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        // whitelistedDomains: ['localhost:4200'],
        // blacklistedRoutes: ['http://localhost:4200/api/auth']
      }
    })
  ],
  exports: [
    BrowserModule,
  ],
  providers: [
    LoginService,
    AuthGuard,
    RoleGuardService,
    UsersService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
