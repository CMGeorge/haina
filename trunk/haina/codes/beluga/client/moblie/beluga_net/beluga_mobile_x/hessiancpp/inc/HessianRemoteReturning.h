#ifndef	HESSIANREMOTERETURNING_H
#define	HESSIANREMOTERETURNING_H

#include <string>

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

	void	setValue(void* aValue);
	void*	getValue();

private:
	int		statusCode;		//״̬�� ���óɹ�״̬���Ϊ0
	std::string	statusText;		//״̬����
	int		operationCode;	//������
	
	void*	value;		//���ؽ������
};

#endif	/*HESSIANREMOTERETURNING_H*/
