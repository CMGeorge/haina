#ifndef	HESSIANREMOTERETURNING_H
#define	HESSIANREMOTERETURNING_H

#include <string>
#include "json/json.h"

class HessianRemoteReturning
{
private:
	const static long serialVersionUID;

public:
	HessianRemoteReturning();
	~HessianRemoteReturning();

public:
	void	setStatusCode(int aStatusCode);
	int		getStatusCode();

	void	setStatusText(std::string aStatusText);
	std::string	getStatusText();

	void	setOperationCode(int aOperationCode);
	int		getOperationCode();

// 	void	setValue(void* aValue);
// 	void*	getValue();
// 	void	setValue(std::string aValue);
// 	std::string getValue();

	void	setValue(Json::Value aValue);
	Json::Value getValue();


private:
	int		statusCode;		//״̬�� ���óɹ�״̬���Ϊ0
	std::string	statusText;		//״̬����
	int		operationCode;	//������
	
//	void*	value;		//���ؽ������
//	std::string	value;		//���ؽ������
	Json::Value value;	//���ؽ������
};

#endif	/*HESSIANREMOTERETURNING_H*/
