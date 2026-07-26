import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CourseCardComponent } from '../../components/course-card/course-card.component';
import { HighlightDirective } from '../../directives/highlight.directive';
import { CourseService } from '../../services/course.service';
import { Course } from '../../models/course.model';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, CourseCardComponent, HighlightDirective],
  templateUrl: './course-list.component.html'
})
export class CourseListComponent implements OnInit {
  isLoading = true;
  courses: Course[] = [];

  constructor(private courseService: CourseService, private router: Router) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.courses = this.courseService.getCoursesLocal();
      this.isLoading = false;
    }, 1000);
  }

  trackByCourseId(index: number, course: Course): number {
    return course.id;
  }

  onCourseSelect(id: number): void {
    this.router.navigate(['/courses', id]);
  }
}