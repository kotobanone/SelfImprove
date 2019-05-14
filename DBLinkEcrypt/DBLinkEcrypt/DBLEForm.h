#pragma once
#include "Base64Coder.h"

namespace DBLinkEcrypt {

	//using namespace System;
	//using namespace System::ComponentModel;
	//using namespace System::Collections;
	//using namespace System::Windows::Forms;
	//using namespace System::Data;
	//using namespace System::Drawing;

	/// <summary>
	/// DBLEForm ���K�n
	/// </summary>
	public ref class DBLEForm : public System::Windows::Forms::Form
	{
	public:
		DBLEForm(void)
		{
			InitializeComponent();
			//
			//TODO: �b���[�J�غc�禡�{���X
			//
		}

	protected:
		/// <summary>
		/// �M������ϥΤ����귽�C
		/// </summary>
		~DBLEForm()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::Label^  label1;
	protected: 
	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::TextBox^  tB_encrypt;
	private: System::Windows::Forms::TextBox^  tB_decrypt;
	private: System::Windows::Forms::Button^  btn_encrypt;
	private: System::Windows::Forms::Button^  btn_decrypt;

	private:
		/// <summary>
		/// �]�p�u��һݪ��ܼơC
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// �����]�p�u��䴩�һݪ���k - �ФŨϥε{���X�s�边
		/// �ק�o�Ӥ�k�����e�C
		/// </summary>
		void InitializeComponent(void)
		{
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->tB_encrypt = (gcnew System::Windows::Forms::TextBox());
			this->tB_decrypt = (gcnew System::Windows::Forms::TextBox());
			this->btn_encrypt = (gcnew System::Windows::Forms::Button());
			this->btn_decrypt = (gcnew System::Windows::Forms::Button());
			this->SuspendLayout();
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Font = (gcnew System::Drawing::Font(L"�L�n������", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(136)));
			this->label1->Location = System::Drawing::Point(12, 9);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(89, 20);
			this->label1->TabIndex = 0;
			this->label1->Text = L"�[�K�r��G";
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Font = (gcnew System::Drawing::Font(L"�L�n������", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(136)));
			this->label2->Location = System::Drawing::Point(12, 72);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(89, 20);
			this->label2->TabIndex = 1;
			this->label2->Text = L"���X�r��G";
			// 
			// tB_encrypt
			// 
			this->tB_encrypt->Font = (gcnew System::Drawing::Font(L"�L�n������", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(136)));
			this->tB_encrypt->Location = System::Drawing::Point(16, 33);
			this->tB_encrypt->Name = L"tB_encrypt";
			this->tB_encrypt->Size = System::Drawing::Size(376, 29);
			this->tB_encrypt->TabIndex = 2;
			// 
			// tB_decrypt
			// 
			this->tB_decrypt->Font = (gcnew System::Drawing::Font(L"�L�n������", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(136)));
			this->tB_decrypt->Location = System::Drawing::Point(16, 95);
			this->tB_decrypt->Name = L"tB_decrypt";
			this->tB_decrypt->Size = System::Drawing::Size(376, 29);
			this->tB_decrypt->TabIndex = 3;
			// 
			// btn_encrypt
			// 
			this->btn_encrypt->Font = (gcnew System::Drawing::Font(L"�L�n������", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(0)));
			this->btn_encrypt->Location = System::Drawing::Point(408, 32);
			this->btn_encrypt->Name = L"btn_encrypt";
			this->btn_encrypt->Size = System::Drawing::Size(75, 29);
			this->btn_encrypt->TabIndex = 4;
			this->btn_encrypt->Text = L"�[�K";
			this->btn_encrypt->UseVisualStyleBackColor = true;
			this->btn_encrypt->Click += gcnew System::EventHandler(this, &DBLEForm::btn_encrypt_Click);
			// 
			// btn_decrypt
			// 
			this->btn_decrypt->Font = (gcnew System::Drawing::Font(L"�L�n������", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(0)));
			this->btn_decrypt->Location = System::Drawing::Point(408, 94);
			this->btn_decrypt->Name = L"btn_decrypt";
			this->btn_decrypt->Size = System::Drawing::Size(75, 29);
			this->btn_decrypt->TabIndex = 5;
			this->btn_decrypt->Text = L"�ѱK";
			this->btn_decrypt->UseVisualStyleBackColor = true;
			this->btn_decrypt->Click += gcnew System::EventHandler(this, &DBLEForm::btn_decrypt_Click);
			// 
			// DBLEForm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 12);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(495, 154);
			this->Controls->Add(this->btn_decrypt);
			this->Controls->Add(this->btn_encrypt);
			this->Controls->Add(this->tB_decrypt);
			this->Controls->Add(this->tB_encrypt);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->label1);
			this->Name = L"DBLEForm";
			this->Text = L"DBLEForm";
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	Base64Coder B64;
	private: System::Void btn_encrypt_Click(System::Object^  sender, System::EventArgs^  e) {
				 if(tB_decrypt->Text != "")
				 {

				 }
				 else
				 {
					 MessageBox::Show("���X�r�ꬰ�šA�L�k�[�K");
				 }
			 }
private: System::Void btn_decrypt_Click(System::Object^  sender, System::EventArgs^  e) {
			 if(tB_encrypt->Text!="")
			 {

			 }
			 else
			 {
				 MessageBox::Show("�[�K�r�ꬰ�šA�L�k�ѱK");
			 }
		 }
};
}
