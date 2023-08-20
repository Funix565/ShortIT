import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FetchShortComponent } from './fetch-short.component';

describe('FetchShortComponent', () => {
  let component: FetchShortComponent;
  let fixture: ComponentFixture<FetchShortComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FetchShortComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FetchShortComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
