import { createReducer, on } from '@ngrx/store';
import { Course } from '../../models/course.model';
import * as CourseActions from './course.actions';

export interface CourseState {
  courses: Course[];
  loading: boolean;
  error: string | null;
}

export const initialState: CourseState = {
  courses: [],
  loading: false,
  error: null
};

export const courseReducer = createReducer(
  initialState,
  on(CourseActions.loadCourses, state => ({ ...state, loading: true })),
  on(CourseActions.loadCoursesSuccess, (state, { courses }) => ({ ...state, loading: false, courses })),
  on(CourseActions.loadCoursesFailure, (state, { error }) => ({ ...state, loading: false, error }))
);