using UnityEngine;
using UnityEngine.InputSystem;

namespace SaberCombatMeta.Simulation
{
    public class PlayerMovement : MonoBehaviour
    {
        void Update()
        {
            // Проверка 1: Прямой опрос железа в обход InputAction
            if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
            {
                // Если этот лог появится в Player.log — значит, железо работает, 
                // а проблема исключительно в вашем InputAction / Биндинге
                Debug.Log("Низкоуровневый ввод: Клавиша зажата!"); 
            }

            // // Проверка 2: Проверка состояния вашей конкретной кнопки (например, Space или W)
            // // Замените "Movement" и "Jump" на ваши названия из InputAction
            // if (inputActions != null && inputActions.Player.Jump.triggered)
            // {
            //     Debug.Log("Экшен триггернулся из карты ввода!");
            // }
        }
        
        // private MainInputActions inputActions; // Замените на имя вашего сгенерированного класса
        //
        // void Awake()
        // {
        //     inputActions = new YourInputActions();
        // }
        //
        // void OnEnable()
        // {
        //     // Подписываемся на фокус приложения
        //     Application.focusChanged += OnApplicationFocusChanged;
        //
        //     // Если при старте фокус уже есть — включаем
        //     if (Application.isFocused)
        //     {
        //         ResetInput();
        //     }
        // }
        //
        // void OnDisable()
        // {
        //     Application.focusChanged -= OnApplicationFocusChanged;
        //     inputActions.Disable();
        // }
        //
        // private void OnApplicationFocusChanged(bool hasFocus)
        // {
        //     if (hasFocus)
        //     {
        //         // Полностью пересоздаем или перезапускаем экшены при получении фокуса
        //         ResetInput();
        //     }
        //     else
        //     {
        //         inputActions.Disable();
        //     }
        // }
        //
        // private void ResetInput()
        // {
        //     inputActions.Disable();
        //     inputActions.Enable();
        //     Debug.Log("Ввод принудительно перезапущен в фокусе");
        // }
    }
}