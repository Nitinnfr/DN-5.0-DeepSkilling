import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-enrollment-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './enrollment-form.component.html'
})
export class EnrollmentFormComponent {
  submitted = false;

  formData = {
    studentName: '',
    studentEmail: '',
    courseId: null,
    preferredSemester: 'Odd',
    agreeToTerms: false
  };

  onSubmit(form: NgForm): void {
    if (form.valid) {
      console.log('Template Form Value:', form.value);
      this.submitted = true;
    }
  }
}