import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-create-application',
  templateUrl: './create-application.component.html',
  styleUrls: ['./create-application.component.scss']
})
export class CreateApplicationComponent {

  public applicationForm: FormGroup;
  public statuses: Array<string> = ['New', 'Approved', 'Funded'];

  constructor(private formBuilder: FormBuilder) {
    this.applicationForm = this.formBuilder.group({
      firstName: [null],
      lastName: [null],
      phoneNumber: [null],
      email: [null],
      applicationNumber: [null],
      status: [null],
      amount: [null],
      monthlyPayAmount: [null],
      terms: [null],
    });
  }
  ngOnInit(): void {
    this.applicationForm.get('status').setValue(this.statuses[0]);
  }
}
