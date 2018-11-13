import { Component, OnInit, Optional, Inject } from '@angular/core';
import { Encounter } from '../../classes/encounter';
import { MAT_DIALOG_DATA } from '@angular/material';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-modify-encounter',
  templateUrl: './modify-encounter.component.html',
  styleUrls: ['./modify-encounter.component.css']
})
export class ModifyEncounterComponent implements OnInit {

  constructor( @Optional() @Inject(MAT_DIALOG_DATA) public data: Encounter) { }

  modifyEncounterForm;
  encounter: Encounter;

  ngOnInit() {
    this.modifyEncounterForm = new FormControl();
  }

}
