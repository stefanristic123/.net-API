import { Component, EventEmitter, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  passwordsMatch!: boolean;
  maxDate: string | undefined;
  validationErrors!: string[];

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.initializeForm();
    // const today = new Date();
    // const maxDate = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());
    // this.maxDate = maxDate.toISOString().split('T')[0]; 
   }

   initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      const isMatching = control?.value === control?.parent?.get(matchTo)?.value;
      this.passwordsMatch = isMatching;
      return isMatching ? null : { isMatching: true };
    };
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/members'); 
      },
      error: error => {
        this.validationErrors = error;
        console.log(error.error)}
    })
  }

  cancel() {
    this.cancelRegister.emit(false)
  }
}
