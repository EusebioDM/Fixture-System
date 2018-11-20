import { Component, OnInit, Pipe, PipeTransform } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';
import { LoginService } from 'src/app/services/login/login.service';
import { Encounter } from 'src/app/classes/encounter';
import { EncountersService } from 'src/app/services/encounters.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommentIn } from '../../classes/comment-in';
import { InstantErrorStateMatcher } from 'src/app/shared/instant-error-state-matcher';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  constructor(
    private loginService: LoginService,
    private usersService: UsersService,
    private encountersService: EncountersService,
    private formBuilder: FormBuilder
  ) { }

  addCommentForm: FormGroup;
  matcher = new InstantErrorStateMatcher();
  isAdmin: boolean;
  isLoading: boolean;
  error: string;
  userFollowedTeamsEncounters: Array<Encounter>;
  comments: Array<Comment>;

  ngOnInit() {
    this.isAdmin = (this.loginService.getLoggedUserRole() === 'Administrator');
    this.isLoading = true;
    this.getData();
    this.createAddCommentForm();
  }

  private getData() {
    this.usersService.getUserComments().subscribe(
      ((data: Array<Comment>) => { this.comments = data; this.isLoading = false; }),
      ((error: any) => console.log(error))
    );
    this.usersService.getFollowedTeamEncounters().subscribe(
      ((data: Array<Encounter>) => { this.userFollowedTeamsEncounters = data; }),
      ((error: any) => console.log(error))
    );
  }

  createAddCommentForm() {
    this.addCommentForm = this.formBuilder.group({
      encounter: ['',
        Validators.required
      ],
      message: ['',
        Validators.required
      ]
    });
    this.addCommentForm.clearValidators();
  }

  get encounter() {
    return this.addCommentForm.get('encounter');
  }

  get message() {
    return this.addCommentForm.get('message');
  }

  onComment() {
    const comment = this.addCommentForm.value;
    const m = comment.message;

    this.encountersService.addCommentToEncounter(comment.encounter, new CommentIn(m)).subscribe(
      (() => {
        this.ngOnInit();
      }),
      (error) => this.error = error.Message
    );
  }
}

@Pipe({ name: 'encounterLegibleName' })
export class EncounterLegibleNamePipe implements PipeTransform {
  transform(encounter: Encounter): string {
    const encounterTeams = encounter.teamIds;
    const teamsCount = encounterTeams.length;

    let i = 1;
    let singleStringTeams = '';
    encounterTeams.forEach(teamId => {
      const teamName = teamId.split('_');

      if (i === teamsCount) {
        singleStringTeams += teamName[0];
      } else {
        singleStringTeams += teamName[0] + ' vs ';
      }

      i++;
    });

    return singleStringTeams + ' (' + encounter.sportName + ')';
  }
}

@Pipe({
  name: 'encounterLegibleNameAsync',
  pure: false
})
export class EncounterLegibleNameAsync implements PipeTransform {
  private cachedData: string = null;
  private cachedId = '';

  constructor(private encountersService: EncountersService) { }

  transform(encounterId: string): any {
    if (encounterId !== this.cachedId) {
      this.cachedData = null;
      this.cachedId = encounterId;
      this.encountersService.getEnconutersById(encounterId).subscribe(
        ((data: Encounter) => this.transformResultToLegibleData(data)),
        ((error: any) => console.log(error))
      );
  }

    return this.cachedData;
  }

transformResultToLegibleData(encounter: Encounter): void {
  const encounterTeams = encounter.teamIds;
  const teamsCount = encounterTeams.length;

  let i = 1;
  let singleStringTeams = '';
  encounterTeams.forEach(teamId => {
    const teamName = teamId.split('_');

    if (i === teamsCount) {
      singleStringTeams += teamName[0];
    } else {
      singleStringTeams += teamName[0] + ' vs ';
    }

    i++;
  });
  this.cachedData = singleStringTeams + ' (' + encounter.sportName + ')';
}
}
