import { Component, OnInit } from '@angular/core';
import { Encounter } from '../../classes/encounter';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-add-encounters-result',
  templateUrl: './add-encounters-result.component.html',
  styleUrls: ['./add-encounters-result.component.css']
})
export class AddEncountersResultComponent implements OnInit {

  constructor() { }

  encounter: Encounter;
  addEncounterResultsForm;

  ngOnInit() {
    this.addEncounterResultsForm = new FormControl();
  }

}
