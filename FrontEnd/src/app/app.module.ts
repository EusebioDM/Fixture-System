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
import { UsersListComponent } from './components/users/users.component';
import { UsersService } from './services/users.service';
import { HttpModule } from '@angular/http';

/* Angular Material Components */
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCheckboxModule, ErrorStateMatcher, ShowOnDirtyErrorStateMatcher } from '@angular/material';
import { MatButtonModule } from '@angular/material';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTabsModule } from '@angular/material/tabs';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialogModule, MAT_DIALOG_DATA, MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { SportsComponent } from './components/sports/sports.component';
import { TeamsComponent } from './components/teams/teams.component';
//

import { DialogConfirmToDeleteUser } from './components/users/users.component';
import { EncountersComponent } from './components/encounters/encounters.component';
import { AddUserComponent } from './components/add-user/add-user.component';
import { ModifyUserComponent } from './components/modify-user/modify-user.component';
import { AddTeamComponent } from './components/add-team/add-team.component';
import { FollowerNavComponent } from './components/follower-nav/follower-nav.component';
import { FavoritesComponent } from './components/favorites/favorites.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { CompareValidatorDirective } from './shared/compare-validator.directive';

export function tokenGetter() {
  return localStorage.getItem('access_token');
}

const appRoutes: Routes = [
  { path: 'login', component: LoginComponent },
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
    DialogConfirmToDeleteUser,
    EncountersComponent,
    ModifyUserComponent,
    AddTeamComponent,
    FollowerNavComponent,
    FavoritesComponent,
    CalendarComponent,
    CompareValidatorDirective,
  ],
  entryComponents: [
    AddUserComponent,
    AddTeamComponent,
    DialogConfirmToDeleteUser,
    ModifyUserComponent
  ],
  imports: [
    ReactiveFormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    MatCheckboxModule,
    MatCheckboxModule,
    MatButtonModule,
    MatInputModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatStepperModule,
    MatTabsModule,
    MatExpansionModule,
    MatButtonToggleModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatDialogModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    FormsModule,
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
    MatMenuModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
  ],
  providers: [
    LoginService,
    AuthGuard,
    RoleGuardService,
    UsersService,
    { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { hasBackdrop: true } },
    {provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
