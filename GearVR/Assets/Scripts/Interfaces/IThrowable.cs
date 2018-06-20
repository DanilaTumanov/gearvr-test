using UnityEngine;

public interface IThrowable {

    Rigidbody RB { get; }

    void Throw();

}
