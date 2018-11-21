import { Component, OnInit, ViewChild, Pipe, PipeTransform } from '@angular/core';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { Team } from '../../classes/team';
import { TeamsService } from '../../services/teams.service';
import { UsersService } from 'src/app/services/users.service';
import { LoginService } from 'src/app/services/login/login.service';

@Component({
  selector: 'app-follow-teams',
  templateUrl: './follow-teams.component.html',
  styleUrls: ['./follow-teams.component.css']
})
export class FollowTeamsComponent implements OnInit {

  constructor(
    private loginService: LoginService,
    private usersSerivice: UsersService,
    private teamsService: TeamsService
  ) { }

  isAdmin: boolean;
  displayedColumns: string[] = ['teamName', 'sportName', 'actionFollow'];
  displayedColumnsTeamsFollowed: string[] = ['teamName', 'sportName', 'actionUnfollow'];
  dataSource;
  dataSourceFollowedTeams;
  searchKey: string;
  teams: Array<Team>;
  teamsFollowed: Array<Team>;
  teamsUnfollowed: Array<Team>;

  ngOnInit() {
    this.isAdmin = (this.loginService.getLoggedUserRole() === 'Administrator');
    this.getData();
  }

  private getData() {
    this.getAllTeams();
  }

  private getAllTeams() {
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => {
        this.teams = data;
        this.getUserFollowedTeams();
      }),
      ((error: any) => console.log(error))
    );
  }

  private getUserFollowedTeams() {
    this.usersSerivice.getUserFollowedTeams().subscribe(
      ((data: Array<Team>) => {
        this.teamsFollowed = data;
        this.loadTableDataSourceFollowedTeams();
        this.getUnfollowedTeams();
      }),
      ((error: any) => console.log(error))
    );
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Team>(this.teamsUnfollowed);
  }

  private loadTableDataSourceFollowedTeams() {
    this.dataSourceFollowedTeams = new MatTableDataSource<Team>(this.teamsFollowed);
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }

  getUnfollowedTeams() {
    this.teamsUnfollowed = this.teams.filter(i1 => !this.teamsFollowed.some(i2 => i1.name === i2.name && i1.sportName === i2.sportName));
    this.loadTableDataSource();
  }

  onFollow(team: Team) {
    this.teamsService.addFollowedTeamToLoggedUser(team.name + '_' + team.sportName).subscribe(
      () => { this.getData(); }
    );
  }

  onDeleteFollow(team: Team) {
    this.teamsService.deleteFollowedTeamToLoggedUser(team.name + '_' + team.sportName).subscribe(
      () => { this.getData(); }
    );
  }
}
