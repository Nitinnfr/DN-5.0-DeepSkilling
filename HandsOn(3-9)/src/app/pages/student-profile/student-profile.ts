import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EnrollmentService } from '../../services/enrollment.service';
import { Course } from '../../models/course.model';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div style="padding: 1.5rem;">
      <h2>Student Profile</h2>
      <h3>Enrolled Courses:</h3>
      <ul>
        <li *ngFor="let c of enrolledCourses">{{ c.name }} ({{ c.code }})</li>
      </ul>
    </div>
  `
})
export class StudentProfileComponent implements OnInit {
  enrolledCourses: Course[] = [];

  constructor(private enrollmentService: EnrollmentService) {}

  ngOnInit(): void {
    this.enrolledCourses = this.enrollmentService.getEnrolledCourses();
  }
}