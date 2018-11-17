import { Component, OnInit, Pipe, PipeTransform } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';
import { LoginService } from 'src/app/services/login/login.service';
import { Encounter } from 'src/app/classes/encounter';
import { EncountersService } from 'src/app/services/encounters.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommentIn } from '../../classes/comment-in';

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
  isAdmin: boolean;
  userFollowedTeamsEncounters: Array<Encounter>;
  comments: Array<Comment>;

  ngOnInit() {
    this.isAdmin = (this.loginService.getLoggedUserRole() === 'Administrator');
    this.getData();
    this.createAddCommentForm();
  }

  private getData() {
    this.usersService.getUserComments().subscribe(
      ((data: Array<Comment>) => { this.comments = data; }),
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
      () => { this.ngOnInit(); }
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
