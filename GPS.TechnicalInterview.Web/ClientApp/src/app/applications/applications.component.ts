import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.scss']
})
export class ApplicationsComponent {

  public displayedColumns: Array<string> = ['applicationNumber', 'amount', 'dateApplied', 'status']; 
  public applicationForm: FormGroup;

  constructor(private formBuilder: FormBuilder) { 
    this.applicationForm = this.formBuilder.group({
      applicationNumber: [null],
      amount: [null],
      dateApplied: [null],
      status: [null],
    });
  }
}
