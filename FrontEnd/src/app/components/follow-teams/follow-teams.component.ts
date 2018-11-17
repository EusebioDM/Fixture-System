import { Component, OnInit, ViewChild, Pipe, PipeTransform } from '@angular/core';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { Team } from '../../classes/team';
import { TeamsService } from '../../services/teams.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-follow-teams',
  templateUrl: './follow-teams.component.html',
  styleUrls: ['./follow-teams.component.css']
})
export class FollowTeamsComponent implements OnInit {

  constructor(
    private usersSerivice: UsersService,
    private teamsService: TeamsService
  ) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['teamName', 'sportName', 'actionFollow'];
  displayedColumnsTeamsFollowed: string[] = ['teamName', 'sportName', 'actionUnfollow'];
  dataSource;
  dataSourceFollowedTeams;
  searchKey: string;
  temas: Array<Team>;
  teamsFollowed: Array<Team>;

  ngOnInit() {
    this.getData();
  }

  private getData() {

    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => this.result(data)),
      ((error: any) => console.log(error))
    );

    this.usersSerivice.getUserFollowedTeams().subscribe(
      ((data: Array<Team>) => this.resultFollowedTeams(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Team>): void {
    this.temas = data;
    this.loadTableDataSource();
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Team>(this.temas);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  private resultFollowedTeams(data: Array<Team>) {
    this.teamsFollowed = data;
    this.loadTableDataSourceFollowedTeams();
  }

  private loadTableDataSourceFollowedTeams() {
    this.dataSourceFollowedTeams = new MatTableDataSource<Team>(this.teamsFollowed);
    this.dataSourceFollowedTeams.sort = this.sort;
    this.dataSourceFollowedTeams.paginator = this.paginator;
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }

  /*
  public isFollowed(team: Team): boolean {
    const teamId = team.name + '_' + team.sportName;

    if (this.teamsFollowed) {
      this.teamsFollowed.forEach(t => {
        if (t === teamId) {
          return true;
        }
      });
    }

    return false;
  }
  */

  onFollow(team: Team) {
    this.teamsService.addFollowedTeamToLoggedUser(team.name + '_' + team.sportName).subscribe();
  }
}
