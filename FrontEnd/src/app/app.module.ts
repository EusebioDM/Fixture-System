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
import { EncountersComponent } from './components/encounters/encounters.component';
import { AddUserComponent } from './components/add-user/add-user.component';
import { ModifyUserComponent } from './components/modify-user/modify-user.component';
import { AddTeamComponent } from './components/add-team/add-team.component';
import { FollowerNavComponent } from './components/follower-nav/follower-nav.component';
import { FavoritesComponent } from './components/favorites/favorites.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { CompareValidatorDirective } from './shared/compare-validator.directive';
import { UniqueUsernameValidatorDirective } from './shared/unique-username-validator.directive';
import { MaterialModule } from './material/material.module';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { WelcomeNavComponent } from './components/welcome-nav/welcome-nav.component';
import { AddSportComponent } from './components/add-sport/add-sport.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { YesNoDialogComponent } from './components/yes-no-dialog/yes-no-dialog.component';
import { ModifyTeamComponent } from './components/modify-team/modify-team.component';
import { LogsComponent } from './components/logs/logs.component';



export function tokenGetter() {
  return localStorage.getItem('access_token');
}

const appRoutes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    // canActivate: [LoginRedirectService]
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
      expectedRole: 'Follower'
    }
  },
  {
    path: 'favorites',
    component: FavoritesComponent,
    canActivate: [RoleGuardService],
    data: {
      expectedRole: 'Follower'
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
    UniqueUsernameValidatorDirective,
    WelcomeNavComponent,
    AddSportComponent,
    NotFoundComponent,
    YesNoDialogComponent,
    ModifyTeamComponent,
    UserTypePipe,
    SportPlayersPipe,
    LogsComponent
  ],
  entryComponents: [
    AddUserComponent,
    AddSportComponent,
    AddTeamComponent,
    ModifyUserComponent,
    ModifyTeamComponent,
    YesNoDialogComponent
  ],
  imports: [
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
        whitelistedDomains: ['localhost:4200'],
        blacklistedRoutes: ['http://localhost:4200/api/auth']
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
