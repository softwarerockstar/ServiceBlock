using System;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation.Activities
{
    [ExcludeFromCodeCoverage]
    public static class ActivityConstraints
    {
        public static Constraint MustBeInsideOuterActivityConstraint<TOuter, TInner>(string message)
        {
            var activityBeingValidated = new DelegateInArgument<TInner>();
            var validationContext = new DelegateInArgument<ValidationContext>();
            var parent = new DelegateInArgument<Activity>();
            var parentIsOuter = new Variable<bool>();

            return new Constraint<TInner>
            {
                Body = new ActivityAction<TInner, ValidationContext>
                {
                    Argument1 = activityBeingValidated,
                    Argument2 = validationContext,

                    Handler = new Sequence
                    {
                        Variables = 
                        {
                            parentIsOuter
                        },
                        Activities =
                        {
                            new ForEach<Activity>
                            {
                                Values = new GetParentChain 
                                { 
                                    ValidationContext = validationContext 
                                },

                                Body = new ActivityAction<Activity>
                                {
                                    Argument = parent,

                                    Handler = new If
                                    {
                                        Condition = new InArgument<bool>(env => 
                                            object.Equals(parent.Get(env).GetType(), typeof(TOuter))),

                                        Then = new Assign<bool> 
                                        { 
                                            To = parentIsOuter, 
                                            Value = true 
                                        }
                                    }    
                                }
                            },

                            new AssertValidation
                            {
                                Assertion = parentIsOuter,
                                Message = message
                            }
                        }
                    }
                }
            };
        }
    }
}
