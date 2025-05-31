// Login Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Tab functionality
    const signinTab = document.getElementById('signin-tab');
    const signupTab = document.getElementById('signup-tab');
    
    if (signinTab && signupTab) {
        signinTab.addEventListener('click', function() {
            signinTab.classList.add('active');
            signupTab.classList.remove('active');
            // Here you can add logic to show/hide different forms
        });
        
        signupTab.addEventListener('click', function() {
            signupTab.classList.add('active');
            signinTab.classList.remove('active');
            // Here you can add logic to show/hide different forms
            // For now, we'll just show an alert since we only have login
            alert('Sign Up functionality will be implemented soon!');
            // Reset to Sign In tab
            setTimeout(() => {
                signinTab.classList.add('active');
                signupTab.classList.remove('active');
            }, 100);
        });
    }
    
    // Demo account functionality
    const demoButtons = document.querySelectorAll('.demo-btn');
    demoButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            // Add loading state
            const originalText = this.textContent;
            this.textContent = 'Loading...';
            this.disabled = true;
            
            // Re-enable after form submission
            setTimeout(() => {
                this.textContent = originalText;
                this.disabled = false;
            }, 2000);
        });
    });
    
    // Form validation enhancement
    const form = document.querySelector('.login-form');
    const emailInput = document.querySelector('input[name="Email"]');
    const passwordInput = document.querySelector('input[name="Password"]');
    
    if (form && emailInput && passwordInput) {
        // Real-time validation
        emailInput.addEventListener('blur', function() {
            validateEmail(this);
        });
        
        passwordInput.addEventListener('blur', function() {
            validatePassword(this);
        });
        
        // Form submission
        form.addEventListener('submit', function(e) {
            let isValid = true;
            
            if (!validateEmail(emailInput)) {
                isValid = false;
            }
            
            if (!validatePassword(passwordInput)) {
                isValid = false;
            }
            
            if (!isValid) {
                e.preventDefault();
            } else {
                // Add loading state to submit button
                const submitBtn = form.querySelector('.btn-signin');
                if (submitBtn) {
                    submitBtn.textContent = 'Signing In...';
                    submitBtn.disabled = true;
                }
            }
        });
    }
    
    function validateEmail(input) {
        const email = input.value.trim();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (!email) {
            showFieldError(input, 'Email is required');
            return false;
        } else if (!emailRegex.test(email)) {
            showFieldError(input, 'Please enter a valid email address');
            return false;
        } else {
            clearFieldError(input);
            return true;
        }
    }
    
    function validatePassword(input) {
        const password = input.value;
        
        if (!password) {
            showFieldError(input, 'Password is required');
            return false;
        } else if (password.length < 6) {
            showFieldError(input, 'Password must be at least 6 characters');
            return false;
        } else {
            clearFieldError(input);
            return true;
        }
    }
    
    function showFieldError(input, message) {
        clearFieldError(input);
        input.classList.add('is-invalid');
        
        const errorDiv = document.createElement('div');
        errorDiv.className = 'text-danger field-error';
        errorDiv.textContent = message;
        
        input.parentNode.appendChild(errorDiv);
    }
    
    function clearFieldError(input) {
        input.classList.remove('is-invalid');
        const existingError = input.parentNode.querySelector('.field-error');
        if (existingError) {
            existingError.remove();
        }
    }
    
    // Add some nice animations
    const loginCard = document.querySelector('.login-card');
    if (loginCard) {
        loginCard.style.opacity = '0';
        loginCard.style.transform = 'translateY(20px)';
        
        setTimeout(() => {
            loginCard.style.transition = 'all 0.5s ease';
            loginCard.style.opacity = '1';
            loginCard.style.transform = 'translateY(0)';
        }, 100);
    }
});
