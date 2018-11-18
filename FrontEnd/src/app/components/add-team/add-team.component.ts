import { Component, OnInit, Inject } from '@angular/core';
import { TeamsService } from '../../services/teams.service';
import { SportsService } from '../../services/sports.service';
import { Sport } from '../../classes/sport';
import { Team } from 'src/app/classes/team';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { TeamsComponent } from '../teams/teams.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InstantErrorStateMatcher } from 'src/app/shared/instant-error-state-matcher';

@Component({
  selector: 'app-add-team',
  templateUrl: './add-team.component.html',
  styleUrls: ['./add-team.component.css']
})
export class AddTeamComponent implements OnInit {

  error: string;
  logo: string;
  selectedFile;
  sports: Array<Sport>;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: AddTeamComponent,
    private teamsService: TeamsService,
    private sportsService: SportsService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<TeamsComponent>
  ) { }

  addTeamForm: FormGroup;
  matcher = new InstantErrorStateMatcher();

  ngOnInit() {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => this.result(data)),
      ((error: any) => console.log(error))
    );
    this.createAddTeamForm();
  }

  private result(data: Array<Sport>): void {
    this.sports = data;
    console.log(this.sports);
  }

  createAddTeamForm() {
    this.addTeamForm = this.formBuilder.group({
      name: ['',
        Validators.required
      ],
      sportName: ['',
        Validators.required
      ]
    });
  }

  get name() {
    return this.addTeamForm.get('name');
  }

  get sportName() {
    return this.addTeamForm.get('sportName');
  }

  onFileSelected(event) {
    this.selectedFile = event.target.files[0];
    if (this.selectedFile) {
      const reader = new FileReader();

      reader.onload = this.handleReaderLoaded.bind(this);
      reader.readAsBinaryString(this.selectedFile);
    }
  }

  handleReaderLoaded(e) {
    this.logo = btoa(e.target.result);
  }

  public submit() {
    const team = this.addTeamForm.value;
    team.logo = this.logo;

    this.teamsService.addTeam(team).subscribe(
      ((result: Team) => {
        this.dialogRef.close(team);
      }),
      (error) => this.error = error
    );
  }
}
